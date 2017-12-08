using System.Configuration;
using FXA.DPSE.Service.PaymentValidation.Common.Configuration.Elements;

namespace FXA.DPSE.Service.PaymentValidation.Common.Configuration.Section
{
    public class PaymentValidationServiceConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("routingSlip")]
        public ValidationServiceElementCollection ValidationServiceCollection
        {
            get { return ((ValidationServiceElementCollection)(base["routingSlip"])); }
            set { base["routingSlip"] = value; }
        }
    }
}