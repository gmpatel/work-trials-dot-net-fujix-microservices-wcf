using System;
using System.Collections.Generic;

namespace FXA.DPSE.Service.TraceTracking.DataAccess.Core
{
    public interface ITraceTrackingDataAccess
    {
        IList<string> GenerateTraceTrackingNumber(ElectronicTraceTracking electronicTraceTracking, string sessionId);
        bool CheckRequestHasBeenProcessedRecently(int timeOutMiliseconds, ElectronicTraceTracking data, string sessionId);
    }
}