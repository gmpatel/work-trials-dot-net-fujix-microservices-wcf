using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction.Core
{
    public interface IPaymentInstructionWorkflow
    {
        PaymentInstructionFacadeOut<PaymentInstructionResponse> Run(PaymentInstructionRequestWrapper paymentInstructionRequestWrapper);
    }
}