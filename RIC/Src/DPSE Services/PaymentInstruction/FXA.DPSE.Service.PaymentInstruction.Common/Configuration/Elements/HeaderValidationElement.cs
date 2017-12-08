using System.Configuration;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements
{
    public class HeaderValidationElement : ConfigurationElement
    {
        [ConfigurationProperty("operations", DefaultValue = null, IsRequired = true)]
        public string Operations
        {
            get { return (string)base["operations"]; }
            set { base["operations"] = value; }
        }
    }
}
