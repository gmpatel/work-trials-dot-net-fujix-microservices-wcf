using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Chilkat;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Common.Helper;
using System.Collections.Generic;
using System.Threading;
using FXA.DPSE.Service.DipsTransport.Business.Entities;

namespace FXA.DPSE.Service.DipsTransport.Business.SimpleTransport
{
    public class PathToSftpTransportProcessor : BaseTransportProcessor
    {
        public PathToSftpTransportProcessor(TransportElement transport) : base(transport)
        {
        }

        public override DipsTransportBusinessResult Transport(DipsTransportBusinessData data)
        {
            var result = new DipsTransportBusinessResult
            {
                MessageVersion = data.MessageVersion,
                RequestGuid = data.RequestGuid,
                RequestUtc = data.RequestUtc,
                ReportDate = data.ReportDate,
                IpAddressV4 = data.IpAddressV4
            };

            var destinationSftp = default(SFtp);
            var sourcePath = Source.Path;
            var destinationPath = Destination.Path;

            try
            {
                destinationSftp = SftpHelper.GetConnection(Destination.Connection);

                if (string.IsNullOrEmpty(sourcePath) || !Directory.Exists(sourcePath))
                    throw new InvalidOperationException("Invalid source path or source path does not exists");

                if (string.IsNullOrEmpty(destinationPath)) destinationPath = "/";
                destinationPath = destinationPath.Replace(@"\", @"/");

                if (destinationSftp.OpenDir(destinationPath) == null)
                    throw new InvalidOperationException(string.Format("Invalid destination path on SFTP server or destination path/dir does not exists on SFTP server ({0})", destinationSftp.LastErrorText));

                var files = new DirectoryInfo(sourcePath)
                    .GetFiles()
                    .Where(file => Regex.Match(file.Name, Source.RegEx).Success);

                var moveFailedFiles = new List<FileInfo>();
                var moveException = default(Exception);
                var error = false;
                
                foreach (var file in files)
                {
                    var remoteFilePath = string.Format("{0}{1}{2}", destinationPath,
                        (!destinationPath.EndsWith(@"/") ? @"/" : string.Empty), file.Name);
                    var localFilePath = file.FullName.Replace(@"\", @"/");
                    var moved = false;
                    var tries = 0;

                    while (!moved && tries < 5)
                    {
                        try
                        {
                            if (!destinationSftp.IsConnected) 
                                destinationSftp = SftpHelper.GetConnection(Destination.Connection);

                            if (destinationSftp.UploadFileByName(remoteFilePath, localFilePath))
                            {
                                moved = true;

                                result.AddBusinessInfo(
                                    new DipsTransportBusinessInfo (
                                        string.Format(
                                            "File processed/uploaded successfully (Path To SFTP), from, {0}, to, {1} (SFTP: {2})",
                                            file.FullName, remoteFilePath, Destination.Connection.Server)
                                        )
                                    );
                            }
                            else
                            {
                                throw new InvalidOperationException(destinationSftp.LastErrorText);    
                            }
                        }
                        catch (Exception exception)
                        {
                            tries++;
                            moveException = exception;
                            Thread.Sleep(1000);
                        }
                    }

                    if (!moved)
                    {
                        error = true;
                        moveFailedFiles.Add(file);
                        result.AddBusinessWarnings(
                            new List<DpseBusinessWarning>
                            {
                                new DpseBusinessWarning
                                (
                                    DpseBusinessWarningType.Operational, 
                                    string.Format("Failed to process/upload file (with 5 re-tries) (Path To SFTP), from, {0}, to, {1} (SFTP: {2})", file.FullName, remoteFilePath, Destination.Connection.Server)
                                )
                            }
                        );
                    }    
                }

                if (error && moveFailedFiles.Count > 0)
                {
                    throw new IOException(string.Format("Few files were processed/uploaded successfully but unable to process/upload {0} files : {1}", moveFailedFiles.Count, moveException == null ? string.Empty : moveException.Message), moveException);
                }
            }
            catch (Exception exception)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException, "DPSE-?", exception.Message, exception.StackTrace));
            }
            finally
            {
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