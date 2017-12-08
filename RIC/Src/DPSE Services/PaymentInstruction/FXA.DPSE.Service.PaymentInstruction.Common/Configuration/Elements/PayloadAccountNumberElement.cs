using System.Configuration;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements
{
    public class PayloadAccountNumberElement : ConfigurationElement
    {
        [ConfigurationProperty("header", DefaultValue = null, IsRequired = true)]
        public string Header
        {
            get { return (string)base["header"]; }
            set { base["header"] = value; }
        }
    }
}