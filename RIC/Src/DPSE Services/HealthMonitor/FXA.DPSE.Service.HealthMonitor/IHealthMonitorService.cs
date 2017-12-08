using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.HealthMonitor;

namespace FXA.DPSE.Service.HealthMonitor
{
    [ServiceContract]
    public interface IHealthMonitorService
    {
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "post"
        )]
        [OperationContract]
        HealthMonitorPostResponse Post(HealthMonitorPostRequest request);
    }
}