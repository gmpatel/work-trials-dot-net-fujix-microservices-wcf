using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DTO.Core.Response;
using FXA.DPSE.Service.DTO.PaymentValidation;

namespace FXA.DPSE.Service.DTO.ShadowPost
{
    [DataContract]
    public class ShadowPostResponse : DpseResponseBase
    {
        [Required]
        [DataMember(Name = "message_version", Order = 4)]
        public string MessageVersion { get; set; }

        [Required]
        [DataMember(Name = "request_utc", Order = 5)]
        public string RequestUtc { get; set; }

        [Required]
        [DataMember(Name = "request_guid", Order = 6)]
        public string RequestGuid { get; set; }

        [Required]
        [DataMember(Name = "total_transaction_amount", Order = 7)]
        public int TotalTransactionAmount { get; set; }

        [Required]
        [DataMember(Name = "channel_type", Order = 8)]
        public string ChannelType { get; set; }

        [Required]
        [DataMember(Name = "cheque_count", Order = 9)]
        public int ChequeCount { get; set; }

        [Required]
        [DataMember(Name = "client_session", Order = 10)]
        public ClientSession ClientSession { get; set; }

        [Required]
        [DataMember(Name = "cheque_responses", Order = 11, EmitDefaultValue = false)]
        public IList<ChequeResponse> ChequeResponses { get; set; }

        [Required]
        [DataMember(Name = "transaction_responses", Order = 12, EmitDefaultValue = false)]
        public IList<TransactionResponse> TransactionResponses { get; set; }

        [DataMember(Name = "result_status", Order = 13)]
        public string ResultStatus { get; set; }
    }

    [DataContract]
    public class ChequeResponse
    {
        [Required]
        [DataMember(Name = "sequence_id", Order = 1, EmitDefaultValue = false)]
        public int SequenceId { get; set; }

        [Required]
        [DataMember(Name = "tracking_id", Order = 2, EmitDefaultValue = false)]
        public string TrackingId { get; set; }

        [Required]
        [DataMember(Name = "processing_date", Order = 3, EmitDefaultValue = false)]
        public string ProcessingDate { get; set; }

        [Required]
        [DataMember(Name = "cheque_response_code", Order = 2, EmitDefaultValue = false)]
        public string Code { get; set; }

        [Required]
        [DataMember(Name = "cheque_response_description", Order = 3, EmitDefaultValue = false)]
        public string Description { get; set; }
    }

    [DataContract]
    public class TransactionResponse
    {
        [Required]
        [DataMember(Name = "transaction_response_code", Order = 1, EmitDefaultValue = false)]
        public string Code { get; set; }

        [Required]
        [DataMember(Name = "transaction_response_description", Order = 2, EmitDefaultValue = false)]
        public string Description { get; set; }
    }
}
