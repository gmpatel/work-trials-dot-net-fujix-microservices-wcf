using System.Configuration;
using FXA.DPSE.Service.LimitChecking.Common.Configuration.Elements;
using FXA.DPSE.Service.LimitChecking.Common.Configuration.Section;

namespace FXA.DPSE.Service.LimitChecking.Common.Configuration
{
    public class LimitCheckingServiceConfiguration : ILimitCheckingServiceConfiguration
    {
        private readonly LimitCheckingServiceConfigurationSection _serviceConfiguration;

        public LimitCheckingServiceConfiguration()
        {
            object section = ConfigurationManager.GetSection("serviceConfig");
            _serviceConfiguration = (LimitCheckingServiceConfigurationSection) section;
        }

        public TransactionLimit TransactionLimit
        {
            get { return _serviceConfiguration.TransactionLimit; }
        }
    }
}
