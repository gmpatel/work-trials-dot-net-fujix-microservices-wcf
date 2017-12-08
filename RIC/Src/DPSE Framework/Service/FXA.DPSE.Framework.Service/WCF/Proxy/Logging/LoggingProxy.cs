using System;
using System.Net;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DTO.Logging;

namespace FXA.DPSE.Framework.Service.WCF.Proxy.Logging
{
    public class LoggingProxy : ILoggingProxy
    {
        private readonly IFrameworkConfig _frameworkConfig;
        
        public LoggingProxy(IFrameworkConfig frameworkConfig)
        {
            _frameworkConfig = frameworkConfig;
        }

        public BusinessResult LogEventAsync(string trackingId, string logName, string description,
            string logLevel, string value1, string value2, string channelType, string sessionId, string serviceName, string operationName)
        {
            var loggingResult = new BusinessResult();
            var loggingRequest = new LoggingRequest(trackingId, logName, description, channelType, sessionId,
                Environment.MachineName, serviceName, operationName, logLevel);
            loggingRequest.LogLevel = logLevel;
            

            var httpLoggingResponse = HttpClientExtensions.PostSyncAsJson<LoggingRequest,
                LoggingResponse>(_frameworkConfig.Services.LoggingService.Url, loggingRequest);

            var loggingResponse = httpLoggingResponse.Content;

            if (!httpLoggingResponse.Succeeded)
            {
                loggingResult.AddBusinessException(new DpseBusinessException()
                {
                    ExceptionType = DpseBusinessExceptionType.LoggingServiceException,
                    ErrorCode = (loggingResponse != null) ? loggingResponse.Code : "DPSE-2003",
                    Message = (loggingResponse != null) ? loggingResponse.Message : "A critical error occurred processing the request"
                });
            }

            return loggingResult;
        }


        public BusinessResult LogEventAsync(LoggingProxyRequest loggingProxyRequest)
        {
            return LogEventAsync(loggingProxyRequest.TrackingId, loggingProxyRequest.LogName,
                loggingProxyRequest.Description, loggingProxyRequest.LogLevel, loggingProxyRequest.Value1,
                loggingProxyRequest.Value2, loggingProxyRequest.ChannelType, loggingProxyRequest.SessionId,
                loggingProxyRequest.ServiceName, loggingProxyRequest.OperationName);
        }
    }
}