using System.Configuration;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements
{
    public class PayloadBsbNumberElement : ConfigurationElement
    {
        [ConfigurationProperty("header", DefaultValue = null, IsRequired = true)]
        public string Header
        {
            get { return (string)base["header"]; }
            set { base["header"] = value; }
        }

        [ConfigurationProperty("credit", DefaultValue = null, IsRequired = true)]
        public string Credit
        {
            get { return (string)base["credit"]; }
            set { base["credit"] = value; }
        }
    }
}