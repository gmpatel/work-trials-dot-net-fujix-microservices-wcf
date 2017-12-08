using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.TraceTracking;

namespace FXA.DPSE.Service.TraceTracking
{
    [ServiceContract]
    public interface ITraceTrackingService
    {
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "electronictracetracking"
        )]
        [OperationContract]
        ElectronicTraceTrackingResponse ElectronicTraceTracking(ElectronicTraceTrackingRequest request);
    }
}   