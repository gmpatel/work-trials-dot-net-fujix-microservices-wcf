using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.PaymentInstruction
{
    [DataContract]
    public class PaymentInstructionStatusUpdateRequest
    {
        [Required]
        [JsonProperty("message_version")]
        [DataMember(Name = "message_version")]
        public string MessageVersion { get; set; }

        [Required]
        [JsonProperty("request_utc")]
        [DataMember(Name = "request_utc")]
        public string RequestDateTimeUtc { get; set; }

        [Required]
        [JsonProperty("request_guid")]
        [DataMember(Name = "request_guid")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("channel_type")]
        [DataMember(Name = "channel_type")]
        public string ChannelType { get; set; }

        [Required]
        [JsonProperty("tracking_id")]
        [DataMember(Name = "tracking_id")]
        public string TrackingId { get; set; }

        [Required]
        [JsonProperty("status")]
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [Required]
        [JsonProperty("client_session")]
        [DataMember(Name = "client_session")]
        public Session ClientSession { get; set; }
    }
}