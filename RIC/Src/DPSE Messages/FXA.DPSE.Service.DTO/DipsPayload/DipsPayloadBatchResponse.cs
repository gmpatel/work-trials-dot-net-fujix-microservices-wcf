using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DTO.Core.Response;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.DipsPayload
{
    [DataContract]
    public class DipsPayloadBatchResponse : DpseResponseBase
    {
        [Required]
        [JsonProperty("result_status")]
        [DataMember(Name = "result_status")]
        public string ResultStatus { get; set; }
    }
}