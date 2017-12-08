using FXA.DPSE.Service.TraceTracking.Common.Configuration.Elements;

namespace FXA.DPSE.Service.TraceTracking.Common.Configuration
{
    public interface ITraceTrackingServiceConfiguration
    {
        TraceTrackingDuplicateRequestElement TraceTrackingDuplicateEventCheck { get; set; }
    }
}