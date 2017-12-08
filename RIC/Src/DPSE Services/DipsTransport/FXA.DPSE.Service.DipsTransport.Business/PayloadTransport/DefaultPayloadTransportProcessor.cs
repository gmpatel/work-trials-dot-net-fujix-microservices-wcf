using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.DataAccess;
using System.Text.RegularExpressions;
using FXA.DPSE.Framework.Common.Security.SHA1;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.Entities;

namespace FXA.DPSE.Service.DipsTransport.Business.PayloadTransport
{
    public class DefaultPayloadTransportProcessor : BaseTransportProcessor
    {
        private readonly BaseTransportProcessor _transporter;
        private readonly IDipsTransportDataAccess _dataAccess;
        private readonly IDipsTransportZipCreator _zipCreator;
        private readonly IDipsTransportPgpCreator _pgpCreator;
        private readonly IFileSystem _fileSystem;

        public DefaultPayloadTransportProcessor(
            ITransportProcessor transporter, 
            IDipsTransportDataAccess dataAccess,
            IDipsTransportZipCreator zipCreator,
            IDipsTransportPgpCreator pgpCreator,
            IFileSystem fileSystem) 
        : base(((BaseTransportProcessor) transporter).Configuration)
        {
            this._transporter = (BaseTransportProcessor) transporter;
            this._dataAccess = dataAccess;
            this._zipCreator = zipCreator;
            this._pgpCreator = pgpCreator;
            this._fileSystem = fileSystem;
        }

        public override DipsTransportBusinessResult Transport(DipsTransportBusinessData data)
        {
            PickNewPayloads();

            var businessResult = ProcessTransmissions(data);

            return businessResult;
        }

        private void PickNewPayloads()
        {
            try
            {
                var paymentInstructions = _dataAccess.GetBatchedPaymentInstructions();

                if (paymentInstructions != null && paymentInstructions.Count > 0)
                {
                    _dataAccess.CreateTransportTransmissionRecords(paymentInstructions);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private DipsTransportBusinessResult ProcessTransmissions(DipsTransportBusinessData data)
        {
            var businessResult = new DipsTransportBusinessResult
            {
                RequestGuid = data.RequestGuid,
                IpAddressV4 = data.IpAddressV4,
                RequestUtc = data.RequestUtc,
                MessageVersion = data.MessageVersion
            };

            var processedTransmissions = new List<Transmission>();
            var dateTime = DateTime.UtcNow;

            try
            {
                var transmissions = _dataAccess.GetTransportTransmissionsToBeProcessed();

                if (transmissions != null && transmissions.Count > 0)
                {
                    foreach (var transmission in transmissions)
                    {
                        var zipFiles = new List<FileInfo>();
                        var failed = ProcessTransmission(transmission, ref zipFiles);

                        if (failed <= 0 && zipFiles.Count > 0)
                        {
                            var payloadTransportFile = CreateTransmissionPayload(transmission, zipFiles);

                            if (payloadTransportFile != null && _fileSystem.FileExists(payloadTransportFile.FullName))
                            {
                                if (TransportPayload(data, transmission, payloadTransportFile, dateTime))
                                {
                                    processedTransmissions.Add(transmission);
                                    _dataAccess.UpdatePaymentInstructionsTransmitted(transmission, dateTime);
                                }
                            }
                        }
                        else
                        {
                            transmission.Status = "RETRY";
                            transmission.TransportedDateTime = null;
                            transmission.RetryCount++;

                            _dataAccess.UpdateTransmission(transmission);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                businessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.BusinessRule, "DPSE-4003", exception.Message, string.Empty));
            }

            var message = (processedTransmissions.Count > 0)
                ? string.Join(" | ",
                    processedTransmissions.Select(
                        x =>
                            string.Format(
                                "Payload transmission successful (total {3}), TransmissionId = {0}, FileName = {1}, Retries = {2}",
                                x.Id, x.FileName, x.RetryCount, processedTransmissions.Count)))
                : "There are no Payloads transmitted";

            businessResult.AddBusinessInfo(new DipsTransportBusinessInfo(message));
            return businessResult;
        }

        private int ProcessTransmission(Transmission transmission, ref List<FileInfo> zipFiles)
        {
            var result = 0;

            foreach (var transmissionBatch in transmission.TransmissionBatches)
            {
                var zipFile = default(FileInfo);

                if (ProcessTransmissionBatch(transmissionBatch, ref zipFile))
                {
                    zipFiles.Add(zipFile);
                    continue;
                }

                result++;
            }

            return result;
        }

        private bool ProcessTransmissionBatch(TransmissionBatch transmissionBatch, ref FileInfo zipFile)
        {
            if (!string.IsNullOrEmpty(transmissionBatch.FileName) && !string.IsNullOrEmpty(transmissionBatch.FilePath) && _fileSystem.FileExists(transmissionBatch.FilePath))
            {
                zipFile = new FileInfo(
                    transmissionBatch.FilePath
                );
            }
            else
            {
                try
                {
                    var paymentInstruction =
                        _dataAccess.GetPaymentInstruction(transmissionBatch.PaymentInstructionId);

                    var targetDirectory = new DirectoryInfo(paymentInstruction.BatchPath);
                    var targetFiles = _fileSystem.GetFilesFromDirectory(targetDirectory) 
                        .Where(file => Regex.Match(file.Name, Source.RegEx, RegexOptions.IgnoreCase).Success)
                        .ToList();
                    var payloadZipFileName = string.Format("{0}.ZIP",
                        targetDirectory.Name);
                    var payloadZipFilePath = Path.Combine(base.Configuration.TempLocation.Path,
                        payloadZipFileName);

                    var payloadZipFile = _zipCreator.CreateZip(new FileInfo(payloadZipFilePath), targetFiles);

                    transmissionBatch.FileName = payloadZipFile.Name;
                    transmissionBatch.FilePath = payloadZipFile.FullName;

                    _dataAccess.UpdateTransmissionBatch(transmissionBatch);

                    zipFile = payloadZipFile;
                }
                catch (Exception exception)
                {
                    return false;
                }
            }

            return true;
        }

        private FileInfo CreateTransmissionPayload(Transmission transmission, IList<FileInfo> zipFiles)
        {
            var payloadTransportFile = default(FileInfo);

            if (!string.IsNullOrEmpty(transmission.FileName) && !string.IsNullOrEmpty(transmission.FilePath) && _fileSystem.FileExists(transmission.FilePath))
            {
                payloadTransportFile = new FileInfo(transmission.FilePath);
            }
            else
            {
                try
                {
                    var payloadContainerZipFileName = string.Format("DPSE-FXA-PAYLOAD-{0}.ZIP",
                        transmission.Id.ToString("000000000"));
                    var payloadContainerZipFilePath = Path.Combine(base.Configuration.TempLocation.Path,
                        payloadContainerZipFileName);
                    var payloadContainerZipFile = new FileInfo(payloadContainerZipFilePath);

                    if (!_fileSystem.FileExists(payloadContainerZipFilePath))
                    {
                        payloadContainerZipFile = _zipCreator.CreateZip(payloadContainerZipFile, zipFiles);
                    }

                    var payloadContainerZipPgpFileName = string.Format("{0}.PGP",
                        payloadContainerZipFileName);
                    var payloadContainerZipPgpFilePath = Path.Combine(base.Configuration.TempLocation.Path,
                        payloadContainerZipPgpFileName);

                    var payloadContainerZipPgpFile = default(FileInfo);
                    var payloadContainerZipPgpFileSHA = default(string);

                    if (!_fileSystem.FileExists(payloadContainerZipPgpFilePath))
                    {
                        payloadContainerZipPgpFile = _pgpCreator.CreatePgp(payloadContainerZipFile);
                    }
                    else
                    {
                        payloadContainerZipPgpFile = new FileInfo(payloadContainerZipPgpFilePath);
                    }

                    payloadContainerZipPgpFileSHA = _fileSystem.GetSHA1String(payloadContainerZipPgpFile);

                    transmission.FileName = payloadContainerZipPgpFile.Name;
                    transmission.FilePath = payloadContainerZipPgpFile.FullName;
                    transmission.FileSHAHash = payloadContainerZipPgpFileSHA;
                    transmission.Status = "READY";

                    _dataAccess.UpdateTransmission(transmission);

                    payloadTransportFile = payloadContainerZipPgpFile;
                }
                catch (Exception exception)
                {
                    transmission.Status = "RETRY";
                    transmission.TransportedDateTime = null;
                    transmission.RetryCount++;

                    _dataAccess.UpdateTransmission(transmission);

                    payloadTransportFile = null;
                }
            }

            return payloadTransportFile;
        }

        private bool TransportPayload(DipsTransportBusinessData data, Transmission transmission, FileInfo payloadTransportFile, DateTime? dateTime)
        {
            try
            {
                _transporter.Source = new SourceElement
                {
                    Path = payloadTransportFile.DirectoryName,
                    RegEx = payloadTransportFile.Name
                };

                transmission.Status = "COMPLETED";
                transmission.TransportedDateTime = dateTime ?? DateTime.UtcNow;

                _dataAccess.UpdateTransmission(transmission);

                var result = _transporter.Transport(data);

                if (result.HasException && !result.HasInfo)
                {
                    transmission.Status = "RETRY";
                    transmission.TransportedDateTime = null;
                    transmission.RetryCount++;

                    _dataAccess.UpdateTransmission(transmission);

                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}