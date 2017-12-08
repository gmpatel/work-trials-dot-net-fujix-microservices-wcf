using System.Configuration;

namespace FXA.DPSE.Service.DipsPayload.Common.Configuration.Elements
{
    public class PayloadTransactionCodeElement : ConfigurationElement
    {
        [ConfigurationProperty("header", DefaultValue = null, IsRequired = true)]
        public string Header
        {
            get { return (string)base["header"]; }
            set { base["header"] = value; }
        }

        [ConfigurationProperty("debit", DefaultValue = null, IsRequired = true)]
        public string Debit
        {
            get { return (string)base["debit"]; }
            set { base["debit"] = value; }
        }

        [ConfigurationProperty("credit", DefaultValue = null, IsRequired = true)]
        public string Credit
        {
            get { return (string)base["credit"]; }
            set { base["credit"] = value; }
        }
    }
}