namespace FXA.DPSE.Service.PaymentInstruction.Facade.Core
{
    public class PaymentInstructionFacadeIn<TServiceData>
    {
        public PaymentInstructionRequestWrapper PaymentInstructionRequest { get; set; }
        public TServiceData Data { get; set; }
    }
}