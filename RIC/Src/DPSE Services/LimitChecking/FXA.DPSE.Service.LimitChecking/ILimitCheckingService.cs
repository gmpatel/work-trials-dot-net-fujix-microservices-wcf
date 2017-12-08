using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.LimitChecking;

namespace FXA.DPSE.Service.LimitChecking
{
    [ServiceContract]
    public interface ILimitCheckingService
    {
        [OperationContract]
        [WebInvoke(Method="POST" , UriTemplate = "limitchecking",
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json)]
        TransactionLimitResponse Limitchecking(TransactionLimitRequest transactionLimitRequest);
    }
}
