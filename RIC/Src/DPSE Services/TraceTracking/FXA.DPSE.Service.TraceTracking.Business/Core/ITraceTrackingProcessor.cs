using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Service.TraceTracking.Business.BusinessEntities;

namespace FXA.DPSE.Service.TraceTracking.Business.Core
{
    public interface ITraceTrackingProcessor
    {
        TraceTrackingBusinessResult GenerateTraceTrackingNumber(TraceTrackingBusinessData businessData);
        BusinessResult CheckDuplicateRequest(TraceTrackingBusinessData businessData);
    }
}
