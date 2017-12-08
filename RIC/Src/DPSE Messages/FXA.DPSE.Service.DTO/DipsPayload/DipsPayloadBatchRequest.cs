using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.DipsPayload
{
    [DataContract]
    public class DipsPayloadBatchRequest
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
        [JsonProperty("ip_address_v4")]
        [DataMember(Name = "ip_address_v4")]
        public string IpAddressV4 { get; set; }

        [Required]
        [JsonProperty("client_name")]
        [DataMember(Name = "client_name")]
        public string ClientName { get; set; }
    }
}