using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Framework.Service.WCF.Proxy.Logging
{
    public interface ILoggingProxy
    {
        BusinessResult LogEventAsync(string trackingId, string logName, string description,
            string logLevel, string value1, string value2, string channelType, string sessionId, string serviceName, string operationName);
        BusinessResult LogEventAsync(LoggingProxyRequest loggingProxyRequest);
    }

    public class LoggingProxyRequest
    {
        public string TrackingId { get; set; }
        public string LogName { get; set; }
        public string Description { get; set; }
        public string LogLevel { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string ChannelType { get; set; }
        public string SessionId { get; set; }
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
    }

    public enum LogLevel
    {
        Fatal,
        Error,
        Warning,
        Information,
        Debug,
        Verbose
    }
}