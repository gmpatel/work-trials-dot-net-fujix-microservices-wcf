using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.TraceTracking.Business.BusinessEntities;
using FXA.DPSE.Service.TraceTracking.Business.Core;
using FXA.DPSE.Service.TraceTracking.Common;
using FXA.DPSE.Service.TraceTracking.Common.Configuration;
using FXA.DPSE.Service.TraceTracking.DataAccess;
using FXA.DPSE.Service.TraceTracking.DataAccess.Core;

namespace FXA.DPSE.Service.TraceTracking.Business
{
    public class TraceTrackingProcessor : ITraceTrackingProcessor
    {
        private readonly ITraceTrackingServiceConfiguration _serviceConfigurartion;
        private readonly ITraceTrackingDataAccess _traceTrackingDataAccess;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;

        public TraceTrackingProcessor(ITraceTrackingServiceConfiguration serviceConfigurartion
            , ITraceTrackingDataAccess traceTrackingDataAccess
            , ILoggingProxy loggingProxy 
            , IAuditProxy auditProxy)
        {
            _serviceConfigurartion = serviceConfigurartion;
            _traceTrackingDataAccess = traceTrackingDataAccess;
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
        }
        public TraceTrackingBusinessResult GenerateTraceTrackingNumber(TraceTrackingBusinessData businessData)
        {
            var traceTrackingBusinessResult = new TraceTrackingBusinessResult();

            //TODO: Look at SavingChanges() exception types. They need to be handled seperately with different response code.

            try
            {
                var data = GetElectronicTraceTracking(businessData);

                traceTrackingBusinessResult.TrackingNumbers = new List<BusinessEntities.TraceTracking>();
                var traceTrackingIds = _traceTrackingDataAccess.GenerateTraceTrackingNumber(data, businessData.ClientSession.SessionId).ToList();

                traceTrackingIds.ForEach(x => traceTrackingBusinessResult.TrackingNumbers.Add(new BusinessEntities.TraceTracking { TrackingNumber = x }));
            }
            catch(ProcessingException<ProxyError> processingException)
            {
                //TODO: Handle audit exception here.
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(new LoggingProxyRequest()
                     {
                         TrackingId = string.Empty,
                         ChannelType = businessData.ChannelType,
                         Description = exception.Message,
                         LogLevel = LogLevel.Error.ToString(),
                         LogName = "ApplicationException",
                         OperationName = "ElectronicTraceTracking",
                         ServiceName = "ElectronicTraceTracking",
                         SessionId = businessData.ClientSession.SessionId,
                         Value1 = exception.StackTrace,
                         Value2 = string.Empty
                     });
                if (loggingResult.HasException)
                {
                    traceTrackingBusinessResult.AddBusinessException(
                      new DpseBusinessException(DpseBusinessExceptionType.LoggingServiceException,
                          loggingResult.BusinessException.ErrorCode, loggingResult.BusinessException.Message, string.Empty));
                    return traceTrackingBusinessResult;
                }
                traceTrackingBusinessResult.AddBusinessException(
                     new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                         TraceTrackingStatusCodes.InternalServerError, "An error occurred processing the request.", string.Empty));
                return traceTrackingBusinessResult;
            }

            return traceTrackingBusinessResult;
        }

        public BusinessResult CheckDuplicateRequest(TraceTrackingBusinessData businessData)
        {
            var result = new BusinessResult();
            try
            {
                var timeOut = _serviceConfigurartion.TraceTrackingDuplicateEventCheck.TimeOutMiliseconds;
                var data = GetElectronicTraceTracking(businessData);

                //TODO: Look at SavingChanges() exception types. They need to be handled seperately with different response code.
                var requestHasProcessed = _traceTrackingDataAccess.CheckRequestHasBeenProcessedRecently(timeOut, data, businessData.ClientSession.SessionId);

                //TODO: Need to audit the duplicate processing with any kind of result in the future, so the following condition is not required.
                if (requestHasProcessed)
                {
                    //var requestIdentificationAuditResult = _auditProxy.AuditAsync(string.Empty, "TraceTracking",
                    //    "Reading the request which are already processed from ElectronicTraceTracking database",
                    //    string.Empty, businessData.ChannelType, businessData.ClientSession.SessionId,
                    //    Environment.MachineName, "ElectronicTraceTracking", "ElectronicTraceTracking");

                    //if (requestIdentificationAuditResult.HasException)
                    //{
                    //    result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.AuditServiceException,
                    //            requestIdentificationAuditResult.BusinessException.ErrorCode, requestIdentificationAuditResult.BusinessException.Message, string.Empty));
                    //    return result;
                    //}

                    var businessErrorMessage =
                        string.Format("This request is already process in the last {0} miliseconds. ChannelType:{1} SessionId:{2} IpAddressV4:{3} DeviceId:{4} ",
                            timeOut, data.ChannelType, data.ClientSessionId, data.ClientIpAddressV4, data.ClientDeviceId);

                    result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                            TraceTrackingStatusCodes.RequestAlreadyProcessedError, businessErrorMessage, string.Empty));
                }
            }
            catch (Exception exception)
            {
                //TODO: Add tracking Id later.
                var loggingResult = _loggingProxy.LogEventAsync(
                    new LoggingProxyRequest()
                    {
                        TrackingId = string.Empty,
                        ChannelType = businessData.ChannelType,
                        Description = exception.Message,
                        LogLevel = LogLevel.Error.ToString(),
                        LogName = "ApplicationException",
                        OperationName = "ElectronicTraceTracking",
                        ServiceName = "ElectronicTraceTracking",
                        SessionId = businessData.ClientSession.SessionId,
                        Value1 = exception.StackTrace,
                        Value2 = string.Empty
                    });
                    
                if (loggingResult.HasException)
                {
                    result.AddBusinessException(
                      new DpseBusinessException(DpseBusinessExceptionType.LoggingServiceException,
                          loggingResult.BusinessException.ErrorCode, loggingResult.BusinessException.Message, string.Empty));
                    return result;
                }
                result.AddBusinessException(
                     new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                         TraceTrackingStatusCodes.InternalServerError, "An error occurred processing the request.", string.Empty));
                return result;
            }
            return result;
        }

        private static ElectronicTraceTracking GetElectronicTraceTracking(TraceTrackingBusinessData businessData)
        {
            var data = new ElectronicTraceTracking
            {
                RequestId = businessData.RequestId,
                ChannelType = businessData.ChannelType,
                ClientSessionId = businessData.ClientSession.SessionId,
                ClientIpAddressV4 = businessData.ClientSession.IpAddressV4,
                ClientDeviceId = businessData.ClientSession.DeviceId,
                ChequeCount = businessData.ChequeCount,
                TotalTransactionAmount = businessData.TotalTransactionAmount,
                DepositAccountName = businessData.DepositingAccountDetails.AccountName,
                DepositAccountNumber = businessData.DepositingAccountDetails.AccountNumber,
                DepositAccountType = businessData.DepositingAccountDetails.AccountType,
                DepositAccountBsbCode = businessData.DepositingAccountDetails.BsbCode,
                DateTimeCreated = DateTime.UtcNow
            };
            return data;
        }
    }
}