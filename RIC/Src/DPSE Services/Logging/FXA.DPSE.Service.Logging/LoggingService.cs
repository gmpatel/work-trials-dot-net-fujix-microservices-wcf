using System;
using System.Net;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DTO.Logging;
using FXA.DPSE.Service.Logging.Business;
using FXA.DPSE.Service.Logging.Common;
using FXA.DPSE.Service.Logging.Business.BusinessEntity;

namespace FXA.DPSE.Service.Logging
{
    [ErrorBehavior("DPSE-2003")]
    [ValidationBehavior("DPSE-2002")]
    [LoggingBehavior]
    public class LoggingService : DpseServiceBase, ILoggingService
    {
        private readonly ILoggingBusiness _loggingBusinessService;

        public LoggingService(ILoggingBusiness loggingBusinessService)
        {
            _loggingBusinessService = loggingBusinessService;
        }

        public LoggingResponse Logging(LoggingRequest loggingRequest)
        {
            try
            {
                var eventLog = new EventLog()
                {
                    ChannelType = loggingRequest.ChannelType,
                    Description = loggingRequest.Description,
                    LogLevel = loggingRequest.LogLevel,
                    MachineName = loggingRequest.MachineName,
                    Name = loggingRequest.Name,
                    OperationName = loggingRequest.OperationName,
                    ServiceName = loggingRequest.ServiceName,
                    SessionId = loggingRequest.SessionId,
                    TrackingId = loggingRequest.TrackingId,
                    Value1 = loggingRequest.Value1,
                    Value2 = loggingRequest.Value2
                };
                var loggingResult = _loggingBusinessService.Log(eventLog);
                if (loggingResult.HasException)
                {
                    return DpseResponse(new LoggingResponse()
                    {
                        TrackingId = loggingRequest.TrackingId,
                        Code = loggingResult.Code,
                        Message = loggingResult.Message
                    },
                        (loggingResult.BusinessException.ExceptionType == DpseBusinessExceptionType.BusinessRule)
                            ? HttpStatusCode.BadRequest
                            : HttpStatusCode.InternalServerError);
                }

                return DpseResponse(new LoggingResponse()
                {
                    TrackingId = loggingRequest.TrackingId,
                    Code = StatusCode.Success,
                    Message = "Log successfully saved", 
                }, HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                //No Logging !
                return DpseResponse(new LoggingResponse()
                {
                    TrackingId = loggingRequest.TrackingId,
                    Code = StatusCode.InternalProcessingError,
                    Message = "An error occurred processing the request"
                }, HttpStatusCode.InternalServerError);
            }
        }
    }
}