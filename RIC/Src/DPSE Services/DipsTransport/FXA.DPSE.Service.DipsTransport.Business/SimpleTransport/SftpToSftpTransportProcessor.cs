using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Chilkat;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Common.Helper;

namespace FXA.DPSE.Service.DipsTransport.Business.SimpleTransport
{
    public class SftpToSftpTransportProcessor : BaseTransportProcessor
    {
        public SftpToSftpTransportProcessor(TransportElement transport) : base(transport)
        {
        }

        public override DipsTransportBusinessResult Transport(DipsTransportBusinessData data)
        {
            var result = new DipsTransportBusinessResult();

            var sourceSftp = default(SFtp);
            var sourcePath = Source.Path;
            var destinationSftp = default(SFtp);
            var destinationPath = Destination.Path;

            try
            {
                sourceSftp = SftpHelper.GetConnection(Source.Connection);
                destinationSftp = SftpHelper.GetConnection(Destination.Connection);

                if (string.IsNullOrEmpty(sourcePath)) sourcePath = "/";
                sourcePath = sourcePath.Replace(@"\", @"/");
                
                if (string.IsNullOrEmpty(destinationPath)) destinationPath = "/";
                destinationPath = destinationPath.Replace(@"\", @"/");
                
                var handle = sourceSftp.OpenDir(sourcePath);
                var dirListing = sourceSftp.ReadDir(handle);
                var files = new List<string>();

                for (var i = 0; i < dirListing.NumFilesAndDirs; i++)
                {
                    files.Add(string.Format("{0}", dirListing.GetFileObject(i).Filename));
                }

                foreach (var file in files.Where(name => Regex.Match(name, Source.RegEx).Success))
                {
                    var remoteSourceFilePath = string.Format("{0}{1}{2}", sourcePath, (!sourcePath.EndsWith(@"/") ? @"/" : string.Empty), file);
                    var remoteDestinationFilePath = string.Format("{0}{1}{2}", destinationPath, (!destinationPath.EndsWith(@"/") ? @"/" : string.Empty), file);
                    var localFilePath = file;

                    if (sourceSftp.DownloadFileByName(remoteSourceFilePath, localFilePath))
                    {
                        sourceSftp.RemoveFile(remoteSourceFilePath);
                    }

                    if (destinationSftp.UploadFileByName(remoteDestinationFilePath, localFilePath))
                    {
                        File.Delete(localFilePath);
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

                if (destinationSftp != null && destinationSftp.IsConnected)
                {
                    destinationSftp.Disconnect();
                    destinationSftp.Dispose();
                }
            }

            return result;
        }
    }
}