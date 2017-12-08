using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.ShadowPost;

namespace FXA.DPSE.Service.ShadowPost
{
    [ServiceContract]
    public interface IShadowPostService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "shadowpost",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ShadowPostResponse ShadowPost(ShadowPostRequest shadowPostRequest);
    }
}
