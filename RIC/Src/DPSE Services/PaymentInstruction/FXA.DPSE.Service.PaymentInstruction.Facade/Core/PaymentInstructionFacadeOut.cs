using FXA.DPSE.Service.PaymentInstruction.Common;

namespace FXA.DPSE.Service.PaymentInstruction.Facade.Core
{
    public class PaymentInstructionFacadeOut<TServiceResponse>
    {
        public TServiceResponse Response { get; set; }
        public bool IsSucceed { get; set; }
        public FacadeErrorType FacadeErrorType { get; set; }

        public string BusinessErrorCode { get; set; }
    }
}