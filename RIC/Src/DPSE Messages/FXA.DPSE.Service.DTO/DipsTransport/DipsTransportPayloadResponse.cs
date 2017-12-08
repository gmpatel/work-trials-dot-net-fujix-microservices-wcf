using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DTO.Core.Response;
using FXA.DPSE.Service.DTO.Core.Validation;

namespace FXA.DPSE.Service.DTO.DipsTransport
{
    [DataContract]
    public class DipsTransportPayloadResponse : DpseResponseBase
    {
        [Required]
        [RequiredWithGuidFormat]
        [DataMember(Name = "request_guid")]
        public string RequestGuid { get; set; }
    }
}