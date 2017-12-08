using System.Configuration;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements
{
    public class PaymentValidationServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = null, IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }
    }
}