using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.TraceTracking
{
    [DataContract]
    public class ElectronicTraceTrackingResponse
    {
        [JsonProperty("code")]
        [DataMember(Name = "code", Order = 2, EmitDefaultValue = false)]
        [Required]
        public string Code { get; set; }

        [JsonProperty("message")]
        [DataMember(Name = "message", Order = 3, EmitDefaultValue = false)]
        [Required]
        public string Message { get; set; }

        [Required]
        [JsonProperty("tracking_ids")]
        [DataMember(Name = "tracking_ids", EmitDefaultValue = false)]
        public IList<TraceTracking> TrackingNumbers { get; set; }
    }

    [DataContract]
    public class TraceTracking 
    {
        [Required]
        [JsonProperty("tracking_id")]
        [DataMember(Name = "tracking_id")]
        public string TrackingNumber { get; set; }
    }
}