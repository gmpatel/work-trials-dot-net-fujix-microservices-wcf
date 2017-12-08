using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Common;
using FXA.DPSE.Service.PaymentValidation.Common.Configuration;
using FXA.DPSE.Service.PaymentValidation.Core;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;

namespace FXA.DPSE.Service.PaymentValidation
{
    public class PaymentValidationMediator : IPaymentValidationMediator
    {
        private readonly IPaymentValidationServiceConfiguration _paymentValidationConfiguration;
        private readonly IValidationComposite _validationComposite;

        public PaymentValidationMediator(IPaymentValidationServiceConfiguration paymentValidationServiceConfiguration, IValidationComposite validationComposite)
        {
            _paymentValidationConfiguration = paymentValidationServiceConfiguration;
            _validationComposite = validationComposite;
        }

        public ValidationResponse ValidatePayload(PaymentValidationRequest validationRequest)
        {
            var validationResponse = new ValidationResponse();

            var routingSlip = GetRoutingSlipByValidationMode(validationRequest);
            foreach (var validationService in routingSlip)
            {
                var validationHandler = _validationComposite.Services[validationService.Name];
                validationResponse = validationHandler.Execute(validationRequest, validationService.Endpoint);

                if (validationResponse.IsSucceed) continue;

                break;
            }

            validationResponse.PaymentValidationResponse.ResultStatus = validationResponse.IsSucceed ? ValidationStatus.ValidationSuccessful : ValidationStatus.ValidationFailure;
            validationResponse.PaymentValidationResponse.Code = validationResponse.IsSucceed ? ResponseCode.ValidationSuccessful : ResponseCode.ValidationFailed;

            return validationResponse;

        }

        private IEnumerable<WorkflowValidationService> GetRoutingSlipByValidationMode(PaymentValidationRequest validationRequest)
        {
            var validationServices = new List<WorkflowValidationService>();
            var validationMode = validationRequest.ValidationMode;
            switch (validationMode.ToLower())
            {
                case "default":
                    validationServices = _paymentValidationConfiguration.RoutingSlip.ValidationServices;
                    break;
                default:
                    validationServices.Add(_paymentValidationConfiguration.RoutingSlip.ValidationServices.Find(e => String.Equals(e.Name, validationMode, StringComparison.CurrentCultureIgnoreCase)));
                    break;
            }
            validationServices = validationServices.OrderBy(e => e.Order).ToList();
            return validationServices;
        }
    }
}
