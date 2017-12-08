using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Validation;

namespace FXA.DPSE.Service.DTO.Logging
{
    [DataContract]
    public class LoggingRequest 
    {
        public LoggingRequest(string trackingId, string name, string description, string channelType, string sessionId, string machineName, string serviceName, string operationName, string logLevel)
        {
            TrackingId = trackingId;
            Name = name;
            Description = description;
            ChannelType = channelType;
            SessionId = sessionId;
            MachineName = machineName;
            ServiceName = serviceName;
            OperationName = operationName;
            LogLevel = logLevel;
        }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "log_level")]
        public string LogLevel { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "service_name")]
        public string ServiceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "operation_name")]
        public string OperationName { get; set; }

        [DataMember(Name = "tracking_id")]
        [DataType(DataType.Text)]
        public string TrackingId { get; set; }

        [DataMember(Name = "value_1")]
        [DataType(DataType.Text)]
        public string Value1 { get; set; }

        [DataMember(Name = "value_2")]
        [DataType(DataType.Text)]
        public string Value2 { get; set; }

        [DataMember(Name = "channel_type")]
        [DataType(DataType.Text)]
        public string ChannelType { get; set; }

        [RequiredWithGuidFormat(false)]
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "machine_name")]
        public string MachineName { get; set; }
    }
}