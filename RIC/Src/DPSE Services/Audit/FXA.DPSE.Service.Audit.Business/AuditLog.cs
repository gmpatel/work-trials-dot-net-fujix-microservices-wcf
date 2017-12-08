using System;

namespace FXA.DPSE.Service.Audit.Business
{
    public class AuditLog
    {
        public string TrackingId { get; set; }
        public string ExternalCorrelationId { get; set; }
        public string DocumentReferenceNumber { get; set; }
        public string AuditDateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Resource { get; set; }
        public string ChannelType { get; set; }
        public string MachineName { get; set; }
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
        public string OperatorName { get; set; }
    }
}