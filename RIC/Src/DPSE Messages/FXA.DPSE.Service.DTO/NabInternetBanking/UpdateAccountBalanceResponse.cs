using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Response;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.NabInternetBanking
{
    [DataContract]
    public class UpdateAccountBalanceResponse : DpseResponseBase
    {
        [Required]
        [JsonProperty("status")]
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public IbStatus Status { get; set; }

        [Required]
        [JsonProperty("dpseReference")]
        [DataMember(Name = "dpseReference")]
        public string DpseReference { get; set; }
        
        [Required]
        [JsonProperty("codeLine")]
        [DataMember(Name = "codeLine")]
        public string CodeLine { get; set; }

        [Required]
        [JsonProperty("id")]
        [DataMember(Name = "id")]
        public string InternetBankingId { get; set; }

        [Required]
        [JsonProperty("dailyLimit")]
        [DataMember(Name = "dailyLimit")]
        public string DailyLimit { get; set; }

        [Required]
        [JsonProperty("transactionLimit")]
        [DataMember(Name = "transactionLimit")]
        public string TransactionLimit { get; set; }

        [Required]
        [JsonProperty("connexRef")]
        [DataMember(Name = "connexRef")]
        public string ConnexRef { get; set; }

        [Required]
        [JsonProperty("settlementDate")]
        [DataMember(Name = "settlementDate")]
        public string SettlementDate { get; set; }
    }

    [DataContract]
    public class IbStatus
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}