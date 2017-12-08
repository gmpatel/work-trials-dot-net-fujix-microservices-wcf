using System;
using System.Collections.Generic;
using System.Globalization;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.ShadowPost.Business.Entities;
using FXA.DPSE.Service.ShadowPost.Common;
using FXA.DPSE.Service.ShadowPost.DataAccess;

namespace FXA.DPSE.Service.ShadowPost.Business
{
    public class ShadowPostBusiness : IShadowPostBusiness
    {
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IShadowPostDataAccess _shadowPostDataAccess;

        public ShadowPostBusiness(ILoggingProxy loggingProxy, IAuditProxy auditProxy, IShadowPostDataAccess shadowPostDataAccess)
        {
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _shadowPostDataAccess = shadowPostDataAccess;
        }

        public ShadowPostBusinessResult ProcessChequeProcessingDate(PayloadInfo payloadInfo, List<ShadowPostedChequeInfo> shadowPostedCheques)
        {
            var businessResult = new ShadowPostBusinessResult();
            try
            {
                foreach (var shadowPostedCheque in shadowPostedCheques)
                {
                    DateTime dateTime;
                    var isValidateDate = DateTime.TryParse(shadowPostedCheque.SettlementDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                    if (isValidateDate)
                    {
                        shadowPostedCheque.SettlementDate = dateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        //Audit ?
                        shadowPostedCheque.SettlementDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    }
                }
                businessResult.ShadowPostedCheques = shadowPostedCheques;
                return businessResult;
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(new LoggingProxyRequest()
                {
                    ChannelType = payloadInfo.ChannelType,
                    Description = exception.Message,
                    LogLevel = LogLevel.Error.ToString(),
                    LogName = "ApplicationException",
                    OperationName = "ShadowPost",
                    ServiceName = "ShadowPost",
                    SessionId = payloadInfo.SessionId,
                    TrackingId = payloadInfo.TrackingId,
                    Value1 = exception.StackTrace,
                    Value2 = string.Empty
                });
                if (loggingResult.HasException)
                {
                    //Do nothing !
                }
                businessResult.ShadowPostedCheques.Clear();
                businessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    StatusCode.InternalProcessingError, exception.Message, string.Empty));
                return businessResult;
            }
        }

        public BusinessResult StoreShadowPostRequest(PayloadInfo payloadInfo)
        {
            var businessResult = new BusinessResult();
            try 
            {
                _shadowPostDataAccess.CreateShadowPost(new DataAccess.ShadowPost
                {
                    SessionId = payloadInfo.SessionId,
                    RequestGuid = (!string.IsNullOrWhiteSpace(payloadInfo.RequestGuid) ? new Guid(payloadInfo.RequestGuid) : Guid.Empty),
                    TrackingId = payloadInfo.TrackingId,
                    DeviceId = payloadInfo.DeviceId,
                    IpAddressV4 = payloadInfo.IpAddressV4,
                    CreatedDateTime = DateTime.UtcNow
                }, payloadInfo.ChannelType, payloadInfo.SessionId);
                return businessResult;
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(new LoggingProxyRequest()
                {
                    ChannelType = payloadInfo.ChannelType,
                    Description = exception.Message,
                    LogLevel = LogLevel.Error.ToString(),
                    LogName = "ApplicationException",
                    OperationName = "ShadowPost",
                    ServiceName = "ShadowPost",
                    SessionId = payloadInfo.SessionId,
                    TrackingId = payloadInfo.TrackingId,
                    Value1 = exception.StackTrace,
                    Value2 = string.Empty
                });
                if (loggingResult.HasException)
                {
                    //Do nothing !
                }
                businessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    StatusCode.InternalProcessingError, exception.Message, string.Empty));
                return businessResult;
            }
        }

        public DuplicateIdentificationBusinessResult CheckRequestDuplication(PayloadInfo payloadInfo)
        {
            var duplicateIdentificationBusinessResult = new DuplicateIdentificationBusinessResult(); 
            try
            {
                duplicateIdentificationBusinessResult.IsDuplicated =
                    _shadowPostDataAccess.FindByRequestId(payloadInfo.RequestGuid,
                        payloadInfo.TrackingId, payloadInfo.ChannelType, payloadInfo.SessionId);
                if (duplicateIdentificationBusinessResult.IsDuplicated)
                {
                    var auditResult = _auditProxy.AuditAsync(new AuditProxyRequest
                    {
                        ChannelType = payloadInfo.ChannelType,
                        TrackingId = payloadInfo.TrackingId,
                        Description = string.Format("RequestId:{0}, TrackingId:{1}, ChequeCount:{2}", payloadInfo.RequestGuid, payloadInfo.TrackingId, payloadInfo.ChequeCount),
                        MachineName = Environment.MachineName,
                        Name = "RequestHasAlreadyProcessed",
                        OperationName = "ShadowPost",
                        ServiceName = "ShadowPost",
                        Resource = string.Empty,
                        SessionId = payloadInfo.SessionId
                    });

                    if (auditResult.HasException)
                    {
                        //Do nothing !
                    }
                }
                return duplicateIdentificationBusinessResult;
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(new LoggingProxyRequest
                {
                    ChannelType = payloadInfo.ChannelType,
                    Description = exception.Message,
                    LogLevel = LogLevel.Error.ToString(),
                    LogName = "ApplicationException",
                    OperationName = "ShadowPost",
                    ServiceName = "ShadowPost",
                    SessionId = payloadInfo.SessionId,
                    TrackingId = payloadInfo.TrackingId,
                    Value1 = exception.StackTrace,
                    Value2 = string.Empty
                });
                if (loggingResult.HasException)
                {
                    //Do nothing !
                }
                duplicateIdentificationBusinessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    StatusCode.InternalProcessingError, exception.Message, string.Empty));
                return duplicateIdentificationBusinessResult;
            }
        }
    }
}