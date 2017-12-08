using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Response;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.PaymentInstruction
{
    [DataContract]
    public class PaymentInstructionResponse : DpseResponseBase
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
        public string RequestGuid { get; set; }
        
        [Required]
        [JsonProperty("result_status")]
        [DataMember(Name = "result_status")]
        public string ResultStatus { get; set; }

        [Required]
        [JsonProperty("total_transaction_amount")]
        [DataMember(Name = "total_transaction_amount")]
        public int TotalTransactionAmount { get; set; }

        [Required]
        [JsonProperty("channel_type")]
        [DataMember(Name = "channel_type")]
        public string ChannelType { get; set; }

        [Required]
        [JsonProperty("cheque_count")]
        [DataMember(Name = "cheque_count")]
        public int ChequeCount { get; set; }

        [Required]
        [JsonProperty("client_session")]
        [DataMember(Name = "client_session")]
        public Session ClientSession { get; set; }

        [Required]
        [JsonProperty("cheque_responses")]
        [DataMember(Name = "cheque_responses", EmitDefaultValue = false)]
        public IList<ChequeResponse> ChequeResponses { get; set; }

        [Required]
        [JsonProperty("transaction_responses")]
        [DataMember(Name = "transaction_responses", EmitDefaultValue = false)]
        public IList<TransactionResponse> TransactionResponses { get; set; }
    }

    [DataContract]
    public class ChequeResponse
    {
        [Required]
        [JsonProperty("sequence_id")]
        [DataMember(Name = "sequence_id")]
        public int SequenceId { get; set; }

        [Required]
        [JsonProperty("cheque_response_code")]
        [DataMember(Name = "cheque_response_code")]
        public string ChequeResponseCode { get; set; }

        [Required]
        [JsonProperty("cheque_response_description")]
        [DataMember(Name = "cheque_response_description")]
        public string ChequeResponseDescription { get; set; }
    }

    [DataContract]
    public class TransactionResponse
    {
        [Required]
        [JsonProperty("transaction_response_code")]
        [DataMember(Name = "transaction_response_code")]
        public string TransactionResponseCode { get; set; }

        [Required]
        [JsonProperty("transaction_response_description")]
        [DataMember(Name = "transaction_response_description")]
        public string TransactionResponseDescription { get; set; }
    }
}