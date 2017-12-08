using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;

namespace FXA.DPSE.Service.PaymentValidation.Core
{
    public interface IPaymentValidationMediator
    {
        ValidationResponse ValidatePayload(PaymentValidationRequest validationRequest);
    }
}