using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Validation;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.TraceTracking
{
    [DataContract]
    public class ElectronicTraceTrackingRequest
    {
        [RequiredWithGuidFormat]
        [JsonProperty("request_guid")]
        [DataMember(Name = "request_guid")]
        public string RequestGuid { get; set; }

        [Required]
        [JsonProperty("channel_type")]
        [DataMember(Name = "channel_type")]
        public string ChannelType { get; set; }

        [Required]
        [JsonProperty("client_session")]
        [DataMember(Name = "client_session")]
        public Session ClientSession { get; set; }

        [Required]
        [JsonProperty("cheque_count")]
        [DataMember(Name = "cheque_count")]
        public int ChequeCount { get; set; }

        [Required]
        [JsonProperty("total_transaction_amount")]
        [DataMember(Name = "total_transaction_amount")]
        public int TotalTransactionAmount { get; set; }

        [Required]
        [JsonProperty("depositing_account_details")]
        [DataMember(Name = "depositing_account_details")]
        public AccountDetails DepositingAccountDetails { get; set; }
    }

    [DataContract]
    public class Session
    {
        [JsonProperty("session_id")]
        [RequiredWithGuidFormat]
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [Required]
        [JsonProperty("device_id")]
        [DataMember(Name = "device_id")]
        public string DeviceId { get; set; }

        [Required]
        [JsonProperty("ip_address_v4")]
        [DataMember(Name = "ip_address_v4")]
        public string IpAddressV4 { get; set; }
    }

    [DataContract]
    public class AccountDetails
    {
        [Required]
        [JsonProperty("account_name")]
        [DataMember(Name = "account_name")]
        public string AccountName { get; set; }

        [Required]
        [JsonProperty("account_number")]
        [DataMember(Name = "account_number")]
        public string AccountNumber { get; set; }

        [Required]
        [JsonProperty("bsb_code")]
        [DataMember(Name = "bsb_code")]
        public string BsbCode { get; set; }

        [Required]
        [JsonProperty("account_type")]
        [DataMember(Name = "account_type")]
        public string AccountType { get; set; }
    }
}