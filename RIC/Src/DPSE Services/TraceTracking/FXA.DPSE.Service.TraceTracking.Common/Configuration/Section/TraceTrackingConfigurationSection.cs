using System.Configuration;
using FXA.DPSE.Service.TraceTracking.Common.Configuration.Elements;

namespace FXA.DPSE.Service.TraceTracking.Common.Configuration.Section
{
    public class TraceTrackingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("traceTrackingDuplicateRequest")]
        public TraceTrackingDuplicateRequestElement TraceTrackingDuplicateEventCheck
        {
            get { return ((TraceTrackingDuplicateRequestElement)(base["traceTrackingDuplicateRequest"])); }
            set { base["traceTrackingDuplicateRequest"] = value; }
        }
    }
}