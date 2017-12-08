using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Validation;

namespace FXA.DPSE.Service.DTO.PaymentValidation
{
    [DataContract]
    public class PaymentValidationRequest
    {
        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "message_version")]
        public string MessageVersion { get; set; } 

        [Required]
        [DataMember(Name = "request_utc")]
        public string RequestUtc { get; set; }

        [Required]
        [DataMember(Name = "validation_mode")]
        public string ValidationMode { get; set; }

        [RequiredWithGuidFormat()]
        [DataMember(Name = "request_guid")]
        public string RequestGuid { get; set; }

        [DataMember(Name = "tracking_id")]
        public string TrackingId { get; set; }

        [Required]
        [DataMember(Name = "total_transaction_amount")]
        public int TotalTransactionAmount { get; set; }

        [Required]
        [DataMember(Name = "channel_type")]
        public string ChannelType { get; set; }

        [Required]
        [DataMember(Name = "cheque_count")]
        public int ChequeCount { get; set; }

        [Required]
        [DataMember(Name = "client_session")]
        public ClientSession ClientSession { get; set; }

        [Required]
        [DataMember(Name = "cheques")]
        public IList<Cheque> Cheques { get; set; }

        [Required]
        [DataMember(Name = "depositing_account_details")]
        public DepositeAccountDetails DepositeAccountDetails { get; set; }
    }

    [DataContract]
    public class DepositeAccountDetails
    {
        [Required]
        [DataMember(Name = "account_names")]
        public IList<AccountNameDetails> AccountNames { get; set; }

        [Required]
        [DataMember(Name = "account_number")]
        public string AccountNumber { get; set; }

        [Required]
        [DataMember(Name = "bsb_code")]
        public string Bsb { get; set; }

        [Required]
        [DataMember(Name = "account_type")]
        public string AccountType { get; set; }
    }

    [DataContract]
    public class AccountNameDetails
    {
        [Required]
        [DataMember(Name = "account_name")]
        public string AccountName { get; set; }
    }

    [DataContract]
    public class ClientSession
    {
        [RequiredWithGuidFormat()]
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [Required]
        [DataMember(Name = "user_id_1")]
        public string UserId1 { get; set; }

        [Required]
        [DataMember(Name = "user_id_2")]
        public string UserId2 { get; set; }

        [Required]
        [DataMember(Name = "device_id")]
        public string DeviceId { get; set; }

        [DataMember(Name = "ip_address_v4")]
        public string IpAddressV4 { get; set; }

        [DataMember(Name = "ip_address_v6")]
        public string IpAddressV6 { get; set; }

        [DataMember(Name = "client_name")]
        public string ClientName { get; set; }

        [DataMember(Name = "client_version")]
        public string ClientVersion { get; set; }

        [DataMember(Name = "os")]
        public string Os { get; set; }

        [DataMember(Name = "os_version")]
        public string OsVersion { get; set; }

        [DataMember(Name = "capture_device")]
        public string CaptureDevice { get; set; }
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