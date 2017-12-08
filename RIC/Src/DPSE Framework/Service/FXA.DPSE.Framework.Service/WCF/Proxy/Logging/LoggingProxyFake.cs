using System;
using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Framework.Service.WCF.Proxy.Logging
{
    public class LoggingProxyFake : ILoggingProxy
    {
        public BusinessResult LogEventAsync(string trackingId, string logName, string description,
            string logLevel, string value1, string value2, string channelType, string sessionId, string serviceName, string operationName)
        {
            return new BusinessResult();
        }

        public BusinessResult LogEventAsync(LoggingProxyRequest loggingProxyRequest)
        {
            return new BusinessResult();
        }
    }
}