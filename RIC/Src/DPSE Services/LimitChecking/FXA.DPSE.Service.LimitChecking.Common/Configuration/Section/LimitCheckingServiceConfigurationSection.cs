using System.Configuration;
using FXA.DPSE.Service.LimitChecking.Common.Configuration.Elements;

namespace FXA.DPSE.Service.LimitChecking.Common.Configuration.Section
{
    public class LimitCheckingServiceConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("TransactionLimit")]
        public TransactionLimit TransactionLimit
        {
            get { return ((TransactionLimit)(base["TransactionLimit"])); }
            set { base["TransactionLimit"] = value; }
        }
    }
}