using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsTransport.Business.SimpleTransport
{
    public class PathToPathTransportProcessor : BaseTransportProcessor
    {
        public PathToPathTransportProcessor(TransportElement transport) : base(transport)
        {
        }

        public override DipsTransportBusinessResult Transport(DipsTransportBusinessData data)
        {
            var result = new DipsTransportBusinessResult();

            try
            {
                var sourcePath = Source.Path;
                var destinationPath = Destination.Path;

                if (string.IsNullOrEmpty(sourcePath) || !Directory.Exists(sourcePath))
                    throw new InvalidOperationException("Invalid Source Path");

                if (string.IsNullOrEmpty(destinationPath) || !Directory.Exists(destinationPath))
                    throw new InvalidOperationException("Invalid Destination Path");

                var files = new DirectoryInfo(sourcePath)
                    .GetFiles()
                    .Where(file => Regex.Match(file.Name, Source.RegEx).Success);

                var moveFailedFiles = new List<FileInfo>();
                var moveException = default(Exception);
                var error = false;
                            
                foreach (var file in files)
                {
                    var destFile = new FileInfo(Path.Combine(destinationPath, file.Name));
                    var moved = false;
                    var tries = 0;

                    while (!moved && tries < 5)
                    {
                        try
                        {
                            file.MoveTo(destFile.FullName);
                            
                            moved = true;
                            result.AddBusinessInfo(
                                new DipsTransportBusinessInfo 
                                (
                                    string.Format("File processed/moved successfully (Path To Path), from, {0}, to, {1}", file.FullName, destFile.FullName)
                                )
                            );
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
                                    string.Format("Failed to process/move file (with 5 re-tries) (Path To Path), from, {0}, to, {1}", file.FullName, destFile.FullName)
                                )
                            }
                        );    
                    }
                }

                if (error && moveFailedFiles.Count > 0)
                {
                    throw new IOException(string.Format("Few files were processed/moved successfully but unable to process/move {0} files : {1}", moveFailedFiles.Count, moveException == null ? string.Empty : moveException.Message), moveException);
                }
            }
            catch (Exception exception)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException, "DPSE-?", exception.Message, exception.StackTrace));
            }

            return result;
        }
    }
}