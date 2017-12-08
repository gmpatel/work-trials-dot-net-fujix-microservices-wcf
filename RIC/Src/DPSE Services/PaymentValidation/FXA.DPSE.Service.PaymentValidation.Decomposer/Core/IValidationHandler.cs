using FXA.DPSE.Service.DTO.PaymentValidation;

namespace FXA.DPSE.Service.PaymentValidation.Decomposer.Core
{
    public interface IValidationHandler
    {
        ValidationResponse Execute(PaymentValidationRequest paymentValidationRequest, string serviceUrl);
    }
}
