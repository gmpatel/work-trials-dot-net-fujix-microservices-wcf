using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;

namespace FXA.DPSE.Service.DTO.Core.Response
{
    [DataContract]
    [Serializable]
    public class DpseResponseBase
    {
        [JsonProperty("tracking_id")]
        [DataMember(Name = "tracking_id", Order = 1, EmitDefaultValue = false)]
        public string TrackingId { get; set; }

        [JsonProperty("external_correlation_id")]
        [DataMember(Name = "external_correlation_id", Order = 2, EmitDefaultValue = false)]
        public string ExternalCorrelationId { get; set; }

        [JsonProperty("document_reference_number")]
        [DataMember(Name = "document_reference_number", Order = 3, EmitDefaultValue = false)]
        public string DocumentReferenceNumber { get; set; }

        [JsonProperty("code")]
        [DataMember(Name = "code", Order = 4, EmitDefaultValue = false)]
        public string Code { get; set; }

        [JsonProperty("message")]
        [DataMember(Name = "message", Order = 5, EmitDefaultValue = false)]
        public string Message { get; set; }        
    }
}