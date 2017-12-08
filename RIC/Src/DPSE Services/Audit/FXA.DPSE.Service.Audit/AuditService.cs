using System;
using System.Net;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.Audit.Business;
using FXA.DPSE.Service.Audit.Common;
using FXA.DPSE.Service.Audit.Common.Configuration;
using FXA.DPSE.Service.DTO.Audit;

namespace FXA.DPSE.Service.Audit
{
    [ErrorBehavior("DPSE-1003")]
    [ValidationBehavior("DPSE-1002")]
    [LoggingBehavior]
    public class AuditService : DpseServiceBase, IAuditService
    {
        private readonly IAuditBusiness _auditBusiness;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditCustomConfig _auditConfig;

        public AuditService(IAuditBusiness auditBusiness, ILoggingProxy loggingProxy, IAuditCustomConfig auditConfig)
        {
            _auditBusiness = auditBusiness;
            _loggingProxy = loggingProxy;
            _auditConfig = auditConfig;
        }

        public AuditResponse Audit(AuditRequest auditRequest)
        {
            try
            {
                var auditResult = _auditBusiness.Audit(new AuditLog()
                    {
                        TrackingId = auditRequest.TrackingId,
                        ExternalCorrelationId = auditRequest.ExternalCorrelationId,
                        DocumentReferenceNumber = auditRequest.DocumentReferenceNumber,
                        AuditDateTime = auditRequest.AuditDateTime,
                        Name = auditRequest.Name,
                        Description = auditRequest.Description,
                        Resource = auditRequest.Resource,
                        ChannelType = auditRequest.ChannelType,
                        MachineName = auditRequest.MachineName,
                        ServiceName = auditRequest.ServiceName,
                        OperationName = auditRequest.OperationName,
                        OperatorName = auditRequest.OperatorName
                    }
                );

                return HandleAuditResponse(auditRequest, auditResult);
            }
            catch (Exception exception)
            {
                var result = _loggingProxy.LogEventAsync(auditRequest.TrackingId, "ApplicationException",
                    exception.Message, LogLevel.Error.ToString(), exception.StackTrace,
                    string.Empty, auditRequest.ChannelType, string.Empty, "Audit", "Audit");

                var responseMessage = result.HasException
                    ? string.Format("Logging Service Failed. An error occurred processing the Audit request. {0}. {1}", exception.Message, (_auditConfig.ResponseSettingsElement.IncludeErrorStackTrace ? exception.StackTrace : string.Empty)).Trim()
                    : "An error occurred processing the request.";

                return DpseResponse(new AuditResponse(auditRequest.TrackingId, auditRequest.ExternalCorrelationId, auditRequest.DocumentReferenceNumber)
                {
                    Code = StatusCode.InternalProcessingError,
                    Message = responseMessage
                }, HttpStatusCode.InternalServerError);
            }
        }

        private AuditResponse HandleAuditResponse(AuditRequest auditRequest, AuditBusinessResult auditResult)
        {
            if (auditResult.HasException)
            {
                return DpseResponse(new AuditResponse(auditRequest.TrackingId, auditRequest.ExternalCorrelationId, auditRequest.DocumentReferenceNumber)
                {
                    Code = auditResult.BusinessException.ErrorCode,
                    Message = auditResult.BusinessException.Message
                }, (auditResult.BusinessException.ExceptionType != DpseBusinessExceptionType.BusinessRule) ? HttpStatusCode.InternalServerError : HttpStatusCode.BadRequest);
            }
            return DpseResponse(new AuditResponse(auditRequest.TrackingId, auditRequest.ExternalCorrelationId, auditRequest.DocumentReferenceNumber)
            {
                Code = StatusCode.AuditSuccessfullSaved,
                Message = "Audit successfully saved"
            }, HttpStatusCode.OK);
        }
    }
}