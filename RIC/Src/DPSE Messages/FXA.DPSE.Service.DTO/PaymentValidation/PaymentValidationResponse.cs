using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Response;

namespace FXA.DPSE.Service.DTO.PaymentValidation
{
    [DataContract]
    public class PaymentValidationResponse : DpseResponseBase
    {
        [DataMember(Name = "message_version", Order = 4)]
        public string MessageVersion { get; set; }

        [DataMember(Name = "request_utc", Order = 5)]
        public string RequestUtc { get; set; }

        [DataMember(Name = "request_guid", Order = 6)]
        public string RequestGuid { get; set; }

        [DataMember(Name = "result_status", Order = 7)]
        public string ResultStatus { get; set; }

        [DataMember(Name = "channel_type", Order = 8)]
        public string ChannelType { get; set; }

        [DataMember(Name = "cheque_count", Order = 9)]
        public int ChequeCount { get; set; }

        [DataMember(Name = "client_session", Order = 10)]
        public ClientSession ClientSession { get; set; }

        [DataMember(Name = "cheque_responses", Order =11, EmitDefaultValue = false)]
        public ChequeResponse[] Cheques { get; set; }

        [DataMember(Name = "transaction_responses", Order = 12, EmitDefaultValue = false)]
        public TransactionResponse[] TransactionResponses { get; set; }
    }

    [DataContract]
    public class ChequeResponse
    {
        [DataMember(Name = "sequence_id", Order = 1)]
        public int SequenceId { get; set; }

        [DataMember(Name = "cheque_response_code", Order = 2)]
        public string Code { get; set; }

        [DataMember(Name = "cheque_response_description", Order = 3)]
        public string Description { get; set; }

    }

    [DataContract]
    public class TransactionResponse
    {
        [DataMember(Name = "transaction_response_code", Order = 1)]
        public string Code { get; set; }

        [DataMember(Name = "transaction_response_description", Order = 2)]
        public string Description { get; set; }

    }
}
