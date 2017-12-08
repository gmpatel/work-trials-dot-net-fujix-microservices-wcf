using System.Configuration;
using FXA.DPSE.Service.TraceTracking.Common.Configuration.Elements;
using FXA.DPSE.Service.TraceTracking.Common.Configuration.Section;

namespace FXA.DPSE.Service.TraceTracking.Common.Configuration
{
    public class TraceTrackingServiceConfiguration : ITraceTrackingServiceConfiguration
    {
        private readonly TraceTrackingConfigurationSection _config;

        public TraceTrackingServiceConfiguration()
        {
            _config = (TraceTrackingConfigurationSection)ConfigurationManager.GetSection("serviceConfig");
        }

        public TraceTrackingDuplicateRequestElement TraceTrackingDuplicateEventCheck
        {
            get
            {
                return _config.TraceTrackingDuplicateEventCheck;    
            }
            set
            {
                _config.TraceTrackingDuplicateEventCheck = value;
            }
        }
    }
}