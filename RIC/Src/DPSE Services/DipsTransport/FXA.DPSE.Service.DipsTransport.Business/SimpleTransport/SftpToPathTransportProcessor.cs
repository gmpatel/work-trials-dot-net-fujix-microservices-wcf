using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Chilkat;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Common.Helper;

namespace FXA.DPSE.Service.DipsTransport.Business.SimpleTransport
{
    public class SftpToPathTransportProcessor : BaseTransportProcessor
    {
        public SftpToPathTransportProcessor(TransportElement transport) : base(transport)
        {
        }

        public override DipsTransportBusinessResult Transport(DipsTransportBusinessData data)
        {
            var result = new DipsTransportBusinessResult();

            var sourceSftp = default(SFtp);
            var sourcePath = Source.Path;
            var destinationPath = Destination.Path;

            try
            {
                sourceSftp = SftpHelper.GetConnection(Source.Connection);

                if (string.IsNullOrEmpty(sourcePath)) sourcePath = "/";
                sourcePath = sourcePath.Replace(@"\", @"/");

                if (string.IsNullOrEmpty(destinationPath) || !Directory.Exists(destinationPath))
                    throw new InvalidOperationException("Invalid Destination Path");

                var handle = sourceSftp.OpenDir(sourcePath);
                var dirListing = sourceSftp.ReadDir(handle);
                var files = new List<string>();

                for (var i = 0; i < dirListing.NumFilesAndDirs; i++)
                {
                    files.Add(string.Format("{0}", dirListing.GetFileObject(i).Filename));
                }

                foreach (var file in files.Where(name => Regex.Match(name, Source.RegEx).Success))
                {
                    var remoteFilePath = string.Format("{0}{1}{2}", sourcePath, (!sourcePath.EndsWith(@"/") ? @"/" : string.Empty), file);
                    var localFilePath = Path.Combine(destinationPath, file).Replace(@"\", @"/");

                    if (sourceSftp.DownloadFileByName(remoteFilePath, localFilePath))
                    {
                        sourceSftp.RemoveFile(remoteFilePath);
                    }
                }
            }
            catch (Exception exception)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException, "DPSE-?", exception.Message, exception.StackTrace));
            }
            finally
            {
                if (sourceSftp != null && sourceSftp.IsConnected)
                {
                    sourceSftp.Disconnect();
                    sourceSftp.Dispose();
                }
            }

            return result;
        }
    }
}