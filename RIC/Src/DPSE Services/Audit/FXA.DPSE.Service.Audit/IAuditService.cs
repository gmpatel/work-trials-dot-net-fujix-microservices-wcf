using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.Audit;

namespace FXA.DPSE.Service.Audit
{
    [ServiceContract]
    public interface IAuditService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "audit",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        AuditResponse Audit(AuditRequest eventInfo);
    }
}