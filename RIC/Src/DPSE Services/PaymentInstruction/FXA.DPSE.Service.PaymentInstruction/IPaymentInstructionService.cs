using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.PaymentInstruction;

namespace FXA.DPSE.Service.PaymentInstruction
{
    [ServiceContract]
    public interface IPaymentInstructionService
    {
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "paymentinstruction"
            )]
        [OperationContract]
        PaymentInstructionResponse PaymentInstruction(PaymentInstructionRequest request);

        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "paymentinstruction/status"
            )]
        [OperationContract]
        PaymentInstructionStatusUpdateResponse Status(PaymentInstructionStatusUpdateRequest request);
    }
}