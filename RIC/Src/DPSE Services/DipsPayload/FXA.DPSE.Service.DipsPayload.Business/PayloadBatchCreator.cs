using System;
using System.IO;
using System.Linq;
using FXA.DPSE.Framework.Common;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DipsPayload.Business.Core;
using FXA.DPSE.Service.DipsPayload.Business.Entity;
using FXA.DPSE.Service.DipsPayload.Common;
using FXA.DPSE.Service.DipsPayload.Common.Configuration;
using FXA.DPSE.Service.DipsPayload.Common.PayloadException;
using FXA.DPSE.Service.DipsPayload.DataAccess;

namespace FXA.DPSE.Service.DipsPayload.Business
{
    public class PayloadBatchCreator : IPayloadBatchCreator
    {
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IPaymentInstructionDataAccess _paymentInstructionDataAccess;
        private readonly IDipsPayloadServiceConfiguration _serviceConfiguration;
        private readonly IDipsPayloadMetadataCreator _dipsPayloadXmlCreator;
        private readonly IDipsPayloadImagesCreator _dipsPayloadImageCreator;
        private readonly IFileSystem _fileSystem;

        public PayloadBatchCreator(
            IPaymentInstructionDataAccess paymentInstructionDataAccess
            , ILoggingProxy loggingProxy
            , IAuditProxy auditProxy
            , IDipsPayloadServiceConfiguration serviceConfiguration
            , IDipsPayloadMetadataCreator dipsPayloadXmlCreator
            , IDipsPayloadImagesCreator dipsPayloadImageCreator
            , IFileSystem fileSystem)
        {
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _paymentInstructionDataAccess = paymentInstructionDataAccess;
            _serviceConfiguration = serviceConfiguration;
            _dipsPayloadXmlCreator = dipsPayloadXmlCreator;
            _dipsPayloadImageCreator = dipsPayloadImageCreator;
            _fileSystem = fileSystem;
        }

        //TODO: Important, Rollback scenario for files and database should be done.
        //      LoggingService Failed !
        //
        public DipsPayloadBusinessResult GeneratePayload()
        {
            var businessResult = new DipsPayloadBusinessResult();
            var paymentInstruction = new PaymentInstruction();
            try
            {
                var queuedPayloads = _paymentInstructionDataAccess.GetPaymentInstructions();

                if (queuedPayloads == null || !queuedPayloads.Any()) return businessResult;

                foreach (var payload in queuedPayloads)
                {
                    paymentInstruction = payload;
                    CreatePayload(payload);
                    businessResult.ProcessedBatchCount++;
                }
                return businessResult;
            }
            catch (ProcessingException<ProxyError> processingException)
            {
                businessResult.AddBusinessException(
                new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    StatusCode.InternalProcessingError, string.Format("Audit is failed. {0}. {1}. {2}. {3}.", processingException.StackTrace, processingException.InnerException != null ? processingException.InnerException.Message : string.Empty, processingException.InnerException != null ? processingException.InnerException.StackTrace : string.Empty),
                    processingException.Message));

                var loggingResult = _loggingProxy.LogEventAsync((paymentInstruction != null && paymentInstruction.TrackingId != null) ? paymentInstruction.TrackingId : string.Empty, "ApplicationException",
                    processingException.Message,
                    LogLevel.Error.ToString(), processingException.StackTrace, string.Empty,
                    (paymentInstruction != null && paymentInstruction.ChannelType != null) ? paymentInstruction.ChannelType : string.Empty,
                    (paymentInstruction != null && paymentInstruction.ClientSession != null) ? paymentInstruction.ClientSession.SessionId : string.Empty,
                    "DipsPayload", "DipsPayloadBatch");

                if (loggingResult.HasException)
                {
                    //TODO: ?
                }
                return businessResult;
            }
            catch (ProcessingException<DipsPayloadErrorType> processingException)
            {
                var result = HandleDatabaseOrFileError(processingException, businessResult, paymentInstruction);
                var payloadResult = new DipsPayloadBusinessResult();
                payloadResult.AddBusinessException(result.BusinessException);
                return payloadResult;
            }
            catch (Exception processingException)
            {
                businessResult.AddBusinessException(
                    new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                        StatusCode.InternalProcessingError, string.Format("An error occurred processing the request. {0}. {1}. {2}. {3}.", processingException.Message, processingException.StackTrace, processingException.InnerException != null ? processingException.InnerException.Message : string.Empty, processingException.InnerException != null ? processingException.InnerException.StackTrace : string.Empty), string.Empty));

                var loggingResult = _loggingProxy.LogEventAsync((paymentInstruction != null && paymentInstruction.TrackingId != null) ? paymentInstruction.TrackingId : string.Empty, "ApplicationException",
                   processingException.Message,
                   LogLevel.Error.ToString(), processingException.StackTrace, string.Empty,
                   (paymentInstruction != null && paymentInstruction.ChannelType != null) ? paymentInstruction.ChannelType : string.Empty,
                   (paymentInstruction != null && paymentInstruction.ClientSession != null) ? paymentInstruction.ClientSession.SessionId : string.Empty,
                   "DipsPayload", "DipsPayloadBatch");

                if (loggingResult.HasException)
                {
                    //TODO: ?
                }

                return businessResult;
            }
        }

        public DipsPayloadSingleBusinessResult GeneratePayload(long paymentInstructionId)
        {
            var businessResult = new DipsPayloadSingleBusinessResult();
            PaymentInstruction paymentInstruction = null;
            try
            {
                paymentInstruction = _paymentInstructionDataAccess.GetPaymentInstruction(paymentInstructionId);

                if (paymentInstruction != null)
                {
                    CreatePayload(paymentInstruction);
                    businessResult.PaymentInstructionId = paymentInstructionId;
                }
                else
                {
                    businessResult.PaymentInstructionId = paymentInstructionId;
                    businessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.BusinessRule, "DPSE-6003", string.Format("There is no payment instruction found with id {0} to be processed.", paymentInstructionId), string.Empty));
                }
                
                return businessResult;
            }
            catch (ProcessingException<DipsPayloadErrorType> processingException)
            {
                var result =  HandleDatabaseOrFileError(processingException, businessResult, paymentInstruction);
                var payloadResult = new DipsPayloadSingleBusinessResult()
                {
                    PaymentInstructionId = paymentInstructionId,
                };
                payloadResult.AddBusinessException(result.BusinessException);
                return payloadResult;
            }
            catch (ProcessingException<ProxyError> processingException)
            {
                businessResult.AddBusinessException(
                new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    StatusCode.InternalProcessingError, string.Format("Audit is failed. {0}. {1}. {2}. {3}.", processingException.Message, processingException.StackTrace, processingException.InnerException != null ? processingException.InnerException.Message : string.Empty, processingException.InnerException != null ? processingException.InnerException.StackTrace : string.Empty),
                    processingException.Message));

                var loggingResult = _loggingProxy.LogEventAsync((paymentInstruction != null) ? paymentInstruction.TrackingId : string.Empty, "ApplicationException",
                    processingException.Message,
                    LogLevel.Error.ToString(), processingException.StackTrace, string.Empty,
                    (paymentInstruction != null) ? paymentInstruction.ChannelType : string.Empty,
                    (paymentInstruction != null) ? paymentInstruction.ClientSession.SessionId : string.Empty,
                    "DipsPayload", "DipsPayloadBatch");

                if (loggingResult.HasException)
                {
                    //TODO: ?
                }
                return businessResult;
            }
            catch (Exception exception)
            {
                businessResult.AddBusinessException(
                    new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                        StatusCode.InternalProcessingError, "An error occurred processing the request", string.Empty));

                var loggingResult = _loggingProxy.LogEventAsync((paymentInstruction != null) ? paymentInstruction.TrackingId : string.Empty,
                        "ApplicationException",
                        exception.Message,
                        LogLevel.Error.ToString(), exception.StackTrace, string.Empty,
                        (paymentInstruction != null) ? paymentInstruction.ChannelType : string.Empty,
                        (paymentInstruction != null) ? paymentInstruction.ClientSession.SessionId : string.Empty,
                        "DipsPayload", "DipsPayloadBatch");

                if (loggingResult.HasException)
                {
                    //TODO: ?
                }
                return businessResult;
            }
        }

        private BusinessResult HandleDatabaseOrFileError(ProcessingException<DipsPayloadErrorType> processingException,
            BusinessResult businessResult, PaymentInstruction paymentInstruction)
        {
            //Ugly code !

            if (processingException.Error == DipsPayloadErrorType.EntityNotFoundError)
            {
                businessResult.AddBusinessException(
                    new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                        StatusCode.PaymentInstructionNotFound, string.Format("{0}, {1}, {2}, {3}", processingException.Message, processingException.StackTrace, processingException.InnerException!= null ? processingException.InnerException.Message : string.Empty, processingException.InnerException != null ? processingException.InnerException.StackTrace : string.Empty), string.Empty));
                return businessResult;
            }

            var errorType = (processingException.Error == DipsPayloadErrorType.DatabaseAccessError) ? "Database" : "FileSystem";

            businessResult.AddBusinessException(
                new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    StatusCode.DatabaseOrFileAccessError, string.Format("{0} access error. {1}. {2}. {3}. {4}.", errorType, processingException.Message, processingException.StackTrace, processingException.InnerException != null ? processingException.InnerException.Message : string.Empty, processingException.InnerException != null ? processingException.InnerException.StackTrace : string.Empty),
                    string.Format("{0}. {1}. {2}. {3}.", processingException.Message, processingException.StackTrace, processingException.InnerException != null ? processingException.InnerException.Message : string.Empty, processingException.InnerException != null ? processingException.InnerException.StackTrace : string.Empty)));

            var trackingId = (processingException.Error == DipsPayloadErrorType.DatabaseAccessError)
                ? string.Empty
                : (paymentInstruction != null) ? paymentInstruction.TrackingId : string.Empty;

            var loggingResult = _loggingProxy.LogEventAsync(trackingId, string.Format("{0} Error", errorType),
                processingException.Message,
                LogLevel.Error.ToString(), processingException.StackTrace, string.Empty,
                (paymentInstruction != null) ? paymentInstruction.ChannelType : string.Empty,
                (paymentInstruction != null) ? paymentInstruction.ClientSession.SessionId : string.Empty,
                "DipsPayload", "DipsPayloadBatch");

            if (loggingResult.HasException)
            {
                //TODO: ?
            }
            return businessResult;
        }

        private void CreatePayload(PaymentInstruction paymentInstruction)
        {
            if (paymentInstruction == null) return;

            var payloadDirectory = GetPayloadDirectory(paymentInstruction);

            if (!_dipsPayloadImageCreator.GeneratePayloadImages(paymentInstruction, payloadDirectory)) return;
             _dipsPayloadXmlCreator.GetScannedBatchMetadata(paymentInstruction, payloadDirectory);

            _paymentInstructionDataAccess.UpdatePaymentInstructionBatchDetails(paymentInstruction.Id, payloadDirectory, DateTime.UtcNow);
        }

        private DirectoryInfo GetPayloadDirectory(PaymentInstruction paymentInstruction)
        {
            var payloadDirectoryName = string.Format("OUTCLEARINGSPKG_{0}_{1}", DateTime.UtcNow.ToString("ddMMyyyy"), paymentInstruction.Id.ToString("0000000000"));
            var payloadDirectoryPath = Path.Combine(_serviceConfiguration.PayloadFileSystemLocation.Path, payloadDirectoryName);

            try
            {
                if (!_fileSystem.DirectoryExists(payloadDirectoryPath))
                    _fileSystem.CreateDirectory(payloadDirectoryPath);
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.DirectoryAccessError,
                    exception, string.Empty);
            }

            var auditResult = _auditProxy.AuditAsync(paymentInstruction.TrackingId, "FileSystemWriteAccess",
                string.Format("Created direcotry {0} for payload", payloadDirectoryName),
                string.Format("Location:{0} DirectoryName:{1}", _serviceConfiguration.PayloadFileSystemLocation.Path,
                    payloadDirectoryName), paymentInstruction.ChannelType, paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (auditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                    auditResult.BusinessException.ErrorCode);

            return new DirectoryInfo(payloadDirectoryPath);
        }
        
    }
}