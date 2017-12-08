using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.NabInternetBanking
{
    [DataContract]
    public class UpdateAccountBalanceRequest
    {
        [Required]
        [JsonProperty("dpseReference")]
        [DataMember(Name = "dpseReference")]
        public string TrackingId { get; set; }

        [Required]
        [JsonProperty("codeLine")]
        [DataMember(Name = "codeLine")]
        public string CodeLine { get; set; }

        [Required]
        [JsonProperty("toAccountId")]
        [DataMember(Name = "toAccountId")]
        public string ToAccountNumber { get; set; }

        [Required]
        [JsonProperty("amount")]
        [DataMember(Name = "amount")]
        public string Amount { get; set; }

        [Required]
        [JsonProperty("statementRef")]
        [DataMember(Name = "statementRef")]
        public string TransactionNarrative { get; set; }

        [Required]
        [JsonProperty("accountHolderNames")]
        [DataMember(Name = "accountHolderNames")]
        public IList<string> AccountHolderNames { get; set; }

        [JsonProperty("responseOverride")]
        [DataMember(Name = "responseOverride", IsRequired = false, EmitDefaultValue = false)]
        public string ResponseOverride { get; set; }
    }
}