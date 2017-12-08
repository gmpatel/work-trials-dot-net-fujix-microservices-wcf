using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.Entities.Serializable;
using FXA.DPSE.Service.DipsTransport.Common.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.TransportException;
using FXA.DPSE.Service.DipsTransport.DataAccess;

namespace FXA.DPSE.Service.DipsTransport.Business
{
    public class DipsTransportMetadataCreator : IDipsTransportMetadataCreator
    {
        private readonly IDipsTransportServiceConfiguration _serviceConfiguration;
        private readonly IDipsTransportMetadataSerializer _dipsPayloadMetadataSerializer;
        private readonly IAuditProxy _auditProxy;

        public DipsTransportMetadataCreator(IDipsTransportServiceConfiguration serviceConfiguration
            , IDipsTransportMetadataSerializer dipsPayloadMetadataSerializer
            , IAuditProxy auditProxy)
        {
            _serviceConfiguration = serviceConfiguration;
            _dipsPayloadMetadataSerializer = dipsPayloadMetadataSerializer;
            _auditProxy = auditProxy;
        }

        public FileInfo GenerateMetadata(DateTime reportDate, IList<Transmission> transmissions, DirectoryInfo directory)
        {
            var dpseEndOfDay = GetDpseEndOfDay(reportDate, transmissions);
            var metadata = _dipsPayloadMetadataSerializer.SerializeWithCustomNamespace<DpseEndOfDay>(dpseEndOfDay);
            var payloadXmlFileInfo = new FileInfo(Path.Combine(directory.FullName, string.Format("DPSE-FXA-EOD-{0}.XML", reportDate.ToString("yyyyMMdd"))));

            try
            {
                File.WriteAllText(payloadXmlFileInfo.FullName, metadata);

                var auditResult = _auditProxy.AuditAsync(string.Empty, "FileSystemWriteAccess",
                string.Format("Created metadata file {0} for payload", string.Format("{0}.XML", string.Empty)),
                string.Format("Location:{0}", string.Empty), string.Empty,
                string.Empty, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

                if (auditResult.HasException)
                    throw new ProcessingException<ProxyError>(ProxyError.AuditError,
                        auditResult.BusinessException.Message,
                        auditResult.BusinessException.ErrorCode);
                
                return payloadXmlFileInfo;
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.FileAccessError,
                    exception.Message, exception, string.Empty);
            }
        }

        private static DpseEndOfDay GetDpseEndOfDay(DateTime reportDate, IList<Transmission> transmissions)
        {
            var dpseEndOfDay = new DpseEndOfDay
            {
                TransmissionDate = reportDate.ToString("yyyy-MM-dd"),
                NumberOfTransmissions = transmissions.Count.ToString(),
                BatchFiles = new List<BatchFile>()
            };

            foreach (var transmission in transmissions)
            {
                dpseEndOfDay.BatchFiles.Add(new BatchFile
                    {
                        BatchFileName = transmission.FileName,
                        Sha2Hash = transmission.FileSHAHash,
                        TransmissionDateTime = (transmission.TransportedDateTime ?? DateTime.MinValue).ToString("yyyy-MM-dd hh:mm:ss").Replace(" ", "T"),
                        TransactionCount = transmission.TransactionCount.ToString(),
                        RetryCount = transmission.RetryCount.ToString()
                    }
                );
            }

            return dpseEndOfDay;
        }
    }
}