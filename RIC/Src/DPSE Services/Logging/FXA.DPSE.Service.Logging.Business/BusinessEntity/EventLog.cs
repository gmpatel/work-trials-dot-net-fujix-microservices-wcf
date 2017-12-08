namespace FXA.DPSE.Service.Logging.Business.BusinessEntity
{
    public class EventLog
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogLevel { get; set; }
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
        public string TrackingId { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string ChannelType { get; set; }
        public string SessionId { get; set; }
        public string MachineName { get; set; }
    }
}
