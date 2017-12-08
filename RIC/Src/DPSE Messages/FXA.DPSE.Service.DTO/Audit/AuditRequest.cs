using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Validation;
namespace FXA.DPSE.Service.DTO.Audit
{
    [DataContract]
    public class AuditRequest
    {
        [DataType(DataType.Text)]
        [DataMember(Name = "tracking_id")]
        public string TrackingId { get; set; }

        [RequiredWithGuidFormat]
        [DataType(DataType.Text)]
        [DataMember(Name = "external_correlation_id")]
        public string ExternalCorrelationId { get; set; }

        [DataType(DataType.Text)]
        [DataMember(Name = "document_reference_number")]
        public string DocumentReferenceNumber { get; set; }

        [RequiredWithJsonUtcDateTimeFormatAttribute]
        [DataType(DataType.Text)]
        [DataMember(Name = "audit_utc")]
        public string AuditDateTime { get; set; }
        
        [Required]
        [DataMember(Name = "name")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [DataMember(Name = "resource")]
        [DataType(DataType.Text)]
        public string Resource { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "channel_type")]
        public string ChannelType { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "machine_name")]
        public string MachineName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "service_name")]
        public string ServiceName { get; set; }

        [DataType(DataType.Text)]
        [DataMember(Name = "operation_name")]
        public string OperationName { get; set; }

        [DataType(DataType.Text)]
        [DataMember(Name = "operator_name")]
        public string OperatorName { get; set; }
    }
}