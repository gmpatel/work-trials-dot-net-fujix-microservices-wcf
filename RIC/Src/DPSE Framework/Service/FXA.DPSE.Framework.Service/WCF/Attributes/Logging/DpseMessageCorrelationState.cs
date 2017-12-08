namespace FXA.DPSE.Framework.Service.WCF.Attributes.Logging
{
    public class DpseMessageCorrelationState
    {
        public string Request { get; set; }
        public string Metadata { get; set; }

        public string TrackingId { get; set; }
        public string SessionId { get; set; }
        public string ChannelType { get; set; }

        public string ServiceName { get; set; }
        public string MethodName { get; set; }

        public string Response { get; set; }

        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}