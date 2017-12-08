using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Service.TraceTracking.Business.BusinessEntities
{
    public class TraceTrackingBusinessResult : BusinessResult
    {
        public IList<TraceTracking> TrackingNumbers { get; set; }
    }

    public class TraceTracking
    {
        public string TrackingNumber { get; set; }
    }
}