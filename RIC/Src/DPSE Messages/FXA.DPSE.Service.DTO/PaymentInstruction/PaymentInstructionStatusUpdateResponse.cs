using FXA.DPSE.Service.DTO.Core.Response;
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
    public class PaymentInstructionStatusUpdateResponse : DpseResponseBase
    {
        [Required]
        [JsonProperty("result_status")]
        [DataMember(Name = "result_status")]
        public string ResultStatus { get; set; }

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
        [JsonProperty("errors")]
        [DataMember(Name = "errors", EmitDefaultValue = false)]
        public IList<Error> Errors { get; set; }
    }

    [DataContract]
    public class Error
    {
        [Required]
        [JsonProperty("error_code")]
        [DataMember(Name = "error_code")]
        public string ErrorCode { get; set; }

        [Required]
        [JsonProperty("error_description")]
        [DataMember(Name = "error_description")]
        public string ErrorDescription { get; set; }
    }
}