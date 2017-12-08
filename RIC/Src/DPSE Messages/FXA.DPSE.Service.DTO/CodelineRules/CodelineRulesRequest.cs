using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Validation;

namespace FXA.DPSE.Service.DTO.CodelineRules
{
    [DataContract]
    public class CodelineRulesRequest
    {
        [DataMember(Name = "tracking_id")]
        public string TrackingId { get; set; }

        [Required]
        [DataMember(Name = "channel_type")]
        public string ChannelType { get; set; }

        [RequiredWithGuidFormat()]
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [Required]
        [DataMember(Name = "cheques")]
        public Cheque[] Cheques { get; set; }
    }

    [DataContract]
    public class Cheque
    {
        [Required]
        [DataMember(Name = "sequence_id")]
        public int SequenceId { get; set; }

        [Required]
        [DataMember(Name = "codeline")]
        public string Codeline { get; set; }

        [Required]
        [DataMember(Name = "cheque_amount")]
        public int ChequeAmount { get; set; }

        [DataMember(Name = "tracking_id")]
        public string TrackingId { get; set; }
    }
}

