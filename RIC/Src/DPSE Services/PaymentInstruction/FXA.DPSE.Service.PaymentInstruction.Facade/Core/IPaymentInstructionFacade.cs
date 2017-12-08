namespace FXA.DPSE.Service.PaymentInstruction.Facade.Core
{
    public interface IPaymentInstructionFacade<TFacadeData, TServiceResponse>
    {
        PaymentInstructionFacadeOut<TServiceResponse> Call(PaymentInstructionFacadeIn<TFacadeData> paymentInstructionFacadeIn);
    }
}
