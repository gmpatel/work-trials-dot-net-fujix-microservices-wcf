using System;
using System.Collections.Generic;
using System.IO;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Service.DipsPayload.Business.Core;
using FXA.DPSE.Service.DipsPayload.Business.Entity.Serializable;
using FXA.DPSE.Service.DipsPayload.Common.Configuration;
using FXA.DPSE.Service.DipsPayload.Common.Extensions;
using FXA.DPSE.Service.DipsPayload.Common.PayloadException;
using FXA.DPSE.Service.DipsPayload.DataAccess;

namespace FXA.DPSE.Service.DipsPayload.Business
{
    public class DipsPayloadMetadataCreator : IDipsPayloadMetadataCreator
    {
        private readonly IDipsPayloadServiceConfiguration _serviceConfiguration;
        private readonly IDipsPayloadMetadataSerializer _dipsPayloadMetadataSerializer;
        private readonly IAuditProxy _auditProxy;

        public DipsPayloadMetadataCreator(IDipsPayloadServiceConfiguration serviceConfiguration
            , IDipsPayloadMetadataSerializer dipsPayloadMetadataSerializer
            , IAuditProxy auditProxy)
        {
            _serviceConfiguration = serviceConfiguration;
            _dipsPayloadMetadataSerializer = dipsPayloadMetadataSerializer;
            _auditProxy = auditProxy;
        }

        public void GetScannedBatchMetadata(PaymentInstruction paymentInstruction, DirectoryInfo payloadDirectory)
        {
            var scannedBatch = GetScannedBatch(paymentInstruction);
            var metadata = _dipsPayloadMetadataSerializer.SerializeWithCustomNamespace(scannedBatch);
            var payloadXmlFileInfo =
                new FileInfo(Path.Combine(payloadDirectory.FullName, string.Format("{0}.XML", payloadDirectory.Name)));

            try
            {
                File.WriteAllText(payloadXmlFileInfo.FullName, metadata);
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.FileAccessError,
                    exception.Message, exception, string.Empty);
            }
            

            var rearImageAuditResult = _auditProxy.AuditAsync(paymentInstruction.TrackingId, "FileSystemWriteAccess",
                string.Format("Created metadata file {0} for payload", string.Format("{0}.XML", payloadDirectory.Name)),
                string.Format("Location:{0}", payloadDirectory.FullName), paymentInstruction.ChannelType,
                paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (rearImageAuditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError,
                    rearImageAuditResult.BusinessException.Message,
                    rearImageAuditResult.BusinessException.ErrorCode);
        }

        private ScannedBatch GetScannedBatch(PaymentInstruction paymentInstruction)
        {
            var scannedBatch = new ScannedBatch
            {
                Client = _serviceConfiguration.PayloadProcessingDetails.BatchClient,
                ProcessingDate = paymentInstruction.CreatedDateTime.ToString("yyyy-MM-dd"),
                BatchNumber = string.Format("{0}{1}{2}", "2", _serviceConfiguration.PayloadProcessingDetails.UnitId.GetLast(2), paymentInstruction.Id),
                BatchType = _serviceConfiguration.PayloadProcessingDetails.BatchType,
                SubBatchType = string.Empty,
                Operator = string.IsNullOrEmpty(_serviceConfiguration.PayloadProcessingDetails.Operator) ? Environment.MachineName : _serviceConfiguration.PayloadProcessingDetails.Operator,
                UnitId = _serviceConfiguration.PayloadProcessingDetails.UnitId,
                ProcessingState = _serviceConfiguration.PayloadProcessingDetails.State,
                CollectingBank = _serviceConfiguration.PayloadProcessingDetails.CollectingBank,
                WorkType = _serviceConfiguration.PayloadProcessingDetails.WorkType,
                CaptureBsb = _serviceConfiguration.PayloadProcessingDetails.CaptureBsb,
                BatchVouchers = new List<BatchVoucher>(),
                Source = _serviceConfiguration.PayloadProcessingDetails.Source
            };

            foreach (var voucher in paymentInstruction.Vouchers)
            {
                if (!voucher.IsNonPostingCheque)
                {
                    scannedBatch.BatchVouchers.Add(new BatchVoucher
                    {
                        RawOCR = string.Empty,
                        RawMICR = string.Empty,
                        MicrFlag = string.Empty,
                        MicrUnprocessableFlag = string.Empty,
                        MicrSuspectFraudFlag = true.ToString(),
                        TraceId = string.Format("{0}{1}", _serviceConfiguration.PayloadProcessingDetails.UnitId, voucher.TrackingId.GetLast(6)),
                        ProcessingDate = (paymentInstruction.ProcessingDateTime == null ? string.Empty : ((DateTime)paymentInstruction.ProcessingDateTime).ToString("yyyy-MM-dd")),
                        DocumentType = voucher.VoucherType,
                        TransactionCode = (voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Header) ? _serviceConfiguration.PayloadTransactionCode.Header : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Debit) ? _serviceConfiguration.PayloadTransactionCode.Debit : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Credit) ? _serviceConfiguration.PayloadTransactionCode.Credit : string.Empty),
                        DocumentReferenceNumber = string.Format("{0}{1}", _serviceConfiguration.PayloadProcessingDetails.DocumentReferenceNumberPreFix, voucher.TrackingId.GetLast(6)),
                        BsbNumber = (voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Header) ? _serviceConfiguration.PayloadBsbNumber.Header : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Debit) ? voucher.BSB : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Credit) ? _serviceConfiguration.PayloadBsbNumber.Credit : string.Empty),
                        AuxDom = (voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Header) ? paymentInstruction.Id.ToString() : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Debit) ? voucher.AuxDom : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Credit) ? string.Empty : string.Empty),
                        ExtraAuxDom = string.Empty,
                        Amount = (voucher.AmountInCents <= 0 ? string.Empty : voucher.AmountInCents.ToString()),
                        AccountNumber = (voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Header) ? _serviceConfiguration.PayloadProcessingDetails.BatchAccountNumber : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Debit) ? voucher.AccountNumber : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Credit) ? paymentInstruction.Account.AccountNumber : string.Empty),
                        InactiveFlag = (voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Header) ? string.Empty : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Debit) ? false.ToString() : voucher.VoucherType.Equals(_serviceConfiguration.PayloadVoucherType.Credit) ? false.ToString() : string.Empty),
                    }
                        );
                }
            }

            return scannedBatch;
        }

    }
}