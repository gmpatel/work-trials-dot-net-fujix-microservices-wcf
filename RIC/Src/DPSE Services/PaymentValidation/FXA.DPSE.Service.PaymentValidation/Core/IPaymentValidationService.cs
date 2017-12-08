using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.PaymentValidation;

namespace FXA.DPSE.Service.PaymentValidation.Core
{
    [ServiceContract]
    public interface IPaymentValidationService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "paymentvalidation",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        PaymentValidationResponse PaymentValidation(PaymentValidationRequest paymentValidationRequest);
    }
}
