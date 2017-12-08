using System;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.Logging.Business.BusinessEntity;
using FXA.DPSE.Service.Logging.Common;

namespace FXA.DPSE.Service.Logging.Business
{
    public class LoggingBusiness : ILoggingBusiness
    {
        private readonly IEventLogWriter _businessLogger;

        public LoggingBusiness(IEventLogWriter businessLogger)
        {
            _businessLogger = businessLogger;
        }

        public LoggingBusinessResult Log(EventLog loggingRequest)
        {
            var result = new LoggingBusinessResult();

            try
            {
                _businessLogger.Log(
                    "{TrackingId}, {Name}, {LogLevel}, {Value1}, {Value2}, {ChannelType}, {SessionId}, {MachineName}, {ServiceName}, {OperationName}, {Description}", 
                    loggingRequest.MachineName,
                    loggingRequest.TrackingId, loggingRequest.Name, loggingRequest.LogLevel, loggingRequest.Value1,
                    loggingRequest.Value2, loggingRequest.ChannelType, loggingRequest.SessionId, loggingRequest.MachineName,
                    loggingRequest.ServiceName, loggingRequest.OperationName, loggingRequest.Description);
            }
            catch (Exception exception)
            {
                //No Logging ?
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    StatusCode.InternalProcessingError, "Logging Service is failed", exception.StackTrace));
            }

            return result;
        }
    }
}