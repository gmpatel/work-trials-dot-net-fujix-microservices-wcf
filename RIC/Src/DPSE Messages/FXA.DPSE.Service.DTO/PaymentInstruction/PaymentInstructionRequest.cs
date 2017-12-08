using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.PaymentInstruction
{
    [DataContract]
    public class PaymentInstructionRequest
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
        public string Id { get; set; }

        [Required]
        [JsonProperty("total_transaction_amount")]
        [DataMember(Name = "total_transaction_amount")]
        public int TotalTransactionAmount { get; set; }

        [Required]
        [JsonProperty("channel_type")]
        [DataMember(Name = "channel_type")]
        public string ChannelType { get; set; }

        [Required]
        [JsonProperty("transaction_narrative")]
        [DataMember(Name = "transaction_narrative")]
        public string TransactionNarrative { get; set; }

        [Required]
        [JsonProperty("cheque_count")]
        [DataMember(Name = "cheque_count")]
        public int ChequeCount { get; set; }
        
        [Required]
        [JsonProperty("client_session")]
        [DataMember(Name = "client_session")]
        public Session ClientSession { get; set; }

        
        [Required]
        [JsonProperty("depositing_account_details")]
        [DataMember(Name = "depositing_account_details")]
        public AccountDetails DepositingAccountDetails { get; set; }

        
        [Required]
        [JsonProperty("notifications")]
        [DataMember(Name = "notifications")]
        public IList<Notification> Notifications { get; set; }
        

        [Required]
        [JsonProperty("posting_cheques")]
        [DataMember(Name = "posting_cheques")]
        public IList<PostingCheque> PostingCheques { get; set; }


        [Required]
        [JsonProperty("non_posting_cheques")]
        [DataMember(Name = "non_posting_cheques")]
        public IList<NonPostingCheque> NonPostingCheques { get; set; }
    }

    [DataContract]
    public class Session
    {
        [Required]
        [JsonProperty("session_id")]
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [Required]
        [JsonProperty("user_id_1")]
        [DataMember(Name = "user_id_1")]
        public string UserId1 { get; set; }

        [Required]
        [JsonProperty("user_id_2")]
        [DataMember(Name = "user_id_2")]
        public string UserId2 { get; set; }
        
        [Required]
        [JsonProperty("device_id")]
        [DataMember(Name = "device_id")]
        public string DeviceId { get; set; }

        [Required]
        [JsonProperty("ip_address_v4")]
        [DataMember(Name = "ip_address_v4")]
        public string IpAddressV4 { get; set; }

        [Required]
        [JsonProperty("ip_address_v6")]
        [DataMember(Name = "ip_address_v6")]
        public string IpAddressV6 { get; set; }

        [Required]
        [JsonProperty("client_name")]
        [DataMember(Name = "client_name")]
        public string ClientName { get; set; }

        [Required]
        [JsonProperty("client_version")]
        [DataMember(Name = "client_version")]
        public string ClientVersion { get; set; }

        [Required]
        [JsonProperty("os")]
        [DataMember(Name = "os")]
        public string Os{ get; set; }

        [Required]
        [JsonProperty("os_version")]
        [DataMember(Name = "os_version")]
        public string OsVersion { get; set; }

        [Required]
        [JsonProperty("capture_device")]
        [DataMember(Name = "capture_device")]
        public string CaptureDevice { get; set; }
    }

    [DataContract]
    public class AccountDetails
    {
        [Required]
        [JsonProperty("account_names")]
        [DataMember(Name = "account_names")]
        public IList<AccountName> Names { get; set; }

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

    [DataContract]
    public class AccountName
    {
        [Required]
        [JsonProperty("account_name")]
        [DataMember(Name = "account_name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class Notification
    {
        [Required]
        [JsonProperty("notification_type")]
        [DataMember(Name = "notification_type")]
        public string NotificationType { get; set; }

        [Required]
        [JsonProperty("notification_info")]
        [DataMember(Name = "notification_info")]
        public string NotificationInfo { get; set; }
    }

    [DataContract]
    public class PostingCheque
    {
        [Required]
        [JsonProperty("sequence_id")]
        [DataMember(Name = "sequence_id")]
        public int SequenceId { get; set; }

        [Required]
        [JsonProperty("codeline")]
        [DataMember(Name = "codeline")]
        public string Codeline { get; set; }

        [Required]
        [JsonProperty("cheque_amount")]
        [DataMember(Name = "cheque_amount")]
        public int ChequeAmount { get; set; }

        [Required]
        [JsonProperty("front_image")]
        [DataMember(Name = "front_image")]
        public string FrontImage { get; set; }

        [Required]
        [JsonProperty("rear_image")]
        [DataMember(Name = "rear_image")]
        public string RearImage { get; set; }

        [Required]
        [JsonProperty("front_image_sha")]
        [DataMember(Name = "front_image_sha")]
        public string FrontImageSha { get; set; }

        [Required]
        [JsonProperty("rear_image_sha")]
        [DataMember(Name = "rear_image_sha")]
        public string RearImageSha { get; set; }
    }

    [DataContract]
    public class NonPostingCheque
    {
        [Required]
        [JsonProperty("sequence_id")]
        [DataMember(Name = "sequence_id")]
        public int SequenceId { get; set; }

        [Required]
        [JsonProperty("codeline")]
        [DataMember(Name = "codeline")]
        public string Codeline { get; set; }

        [Required]
        [JsonProperty("amount")]
        [DataMember(Name = "amount")]
        public int ChequeAmount { get; set; }

        [Required]
        [JsonProperty("front_image")]
        [DataMember(Name = "front_image")]
        public string FrontImage { get; set; }

        [Required]
        [JsonProperty("rear_image")]
        [DataMember(Name = "rear_image")]
        public string RearImage { get; set; }
    }
}