using FXA.DPSE.Service.PaymentValidation.Common.Configuration;

namespace FXA.DPSE.Service.PaymentValidation.Business
{
    public class PaymentValidationBusiness : IPaymentValidationBusiness
    {
        private readonly IPaymentValidationServiceConfiguration _paymentValidationServiceConfiguration;

        public PaymentValidationBusiness(IPaymentValidationServiceConfiguration paymentValidationServiceConfiguration)
        {
            _paymentValidationServiceConfiguration = paymentValidationServiceConfiguration;
        }

        public PaymentValidationResult Dispatch()
        {
            var paymentValidationResult = new PaymentValidationResult();
            return paymentValidationResult;
        }
    }
}


