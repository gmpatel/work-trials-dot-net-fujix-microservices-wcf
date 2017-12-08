using System;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.Audit.Common;
using FXA.DPSE.Service.Audit.Common.Configuration;
using FXA.DPSE.Service.Audit.DataAccess;

namespace FXA.DPSE.Service.Audit.Business
{
    public class AuditBusiness : IAuditBusiness
    {
        private readonly IAuditDataAccess _auditDataAccess;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditCustomConfig _auditConfig;

        public AuditBusiness(IAuditDataAccess auditDataAccess, ILoggingProxy loggingProxy, IAuditCustomConfig auditConfig)
        {
            _auditDataAccess = auditDataAccess;
            _loggingProxy = loggingProxy;
            _auditConfig = auditConfig;
        }

        public AuditBusinessResult Audit(AuditLog auditLog)
        {
            var result = new AuditBusinessResult();
            var auditEntity = auditLog.GetAuditEntity();
            
            try
            {
                var id = _auditDataAccess.Insert(auditEntity);
                return result;
            }
            catch (Exception exception)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                        StatusCode.DatabaseOrFileAccessError,
                        string.Format("An error occurred processing the Audit request. {0}. {1}", exception.Message, (_auditConfig.ResponseSettingsElement.IncludeErrorStackTrace ? exception.StackTrace : string.Empty)).Trim(), 
                        exception.StackTrace));

                var loggingResult = _loggingProxy.LogEventAsync(
                    new LoggingProxyRequest
                    {
                        ChannelType = auditLog.ChannelType,
                        Description = exception.Message,
                        LogLevel = LogLevel.Error.ToString(),
                        LogName = "DatabaseOrFileAccessError",
                        OperationName = "Audit",
                        ServiceName = "Audit",
                        SessionId = string.Empty,
                        TrackingId = auditLog.TrackingId,
                        Value1 = exception.StackTrace,
                        Value2 = string.Empty
                    }
                );

                //if (loggingResult.HasException)
                //{
                //    result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.LoggingServiceException,
                //        loggingResult.BusinessException.ErrorCode, loggingResult.BusinessException.Message, exception.StackTrace));
                //}

                return result;
            }
        }
    }

    public static class BusinessToEntityConverterExtensions
    {
        public static DataAccess.Audit GetAuditEntity(this AuditLog auditLog)
        {
            return new DataAccess.Audit
            {
                TrackingIdentifier = auditLog.TrackingId,
                ExternalCorrelationIdentifier = auditLog.ExternalCorrelationId,
                DocumentReferenceNumber = auditLog.DocumentReferenceNumber,
                AuditDateTime = DateTimeOffset.Parse(auditLog.AuditDateTime),
                Name = auditLog.Name,
                Description = auditLog.Description,
                Resource = auditLog.Resource,
                ChannelType = auditLog.ChannelType,
                MachineName = auditLog.MachineName,
                ServiceName = auditLog.ServiceName,
                OperationName = auditLog.OperationName,
                OperatorName = auditLog.OperatorName,
                CreatedDateTime = DateTime.Now
            };
        }
    }
}

//const string message =
//    "{TrackingIdentifier}," +
//    "{ExternalCorrelationIdentifier}," +
//    "{DocumentReferenceNumber}," +
//    "{AuditDateTime}," +
//    "{Name}," +
//    "{Description}," +
//    "{Resource}," +
//    "{ChannelType}," +
//    "{MachineName}," +
//    "{ServiceName}," +
//    "{OperationName}," +
//    "{OperatorName}";

//_auditLogWriter.Log(message,
//    auditLog.TrackingId,
//    auditLog.ExternalCorrelationId,
//    auditLog.DocumentReferenceNumber,
//    auditLog.AuditDateTime,
//    auditLog.Name,
//    auditLog.Description,
//    auditLog.Resource,
//    auditLog.ChannelType,
//    auditLog.MachineName,
//    auditLog.ServiceName,
//    auditLog.OperationName,
//    auditLog.OperatorName);