using System.Collections.Generic;
using System.Configuration;
using FXA.DPSE.Service.PaymentValidation.Common.Configuration.Elements;
using FXA.DPSE.Service.PaymentValidation.Common.Configuration.Section;

namespace FXA.DPSE.Service.PaymentValidation.Common.Configuration
{
    public class PaymentValidationServiceConfiguration : IPaymentValidationServiceConfiguration
    {
        private readonly PaymentValidationServiceConfigurationSection _paymentValidationServiceConfiguration;

        public PaymentValidationServiceConfiguration()
        {
            if (_paymentValidationServiceConfiguration == null)
            {
                _paymentValidationServiceConfiguration =
                    (PaymentValidationServiceConfigurationSection) ConfigurationManager.GetSection("serviceConfig");
            }
        }

        private RoutingSlip _routingSlip;

        public RoutingSlip RoutingSlip
        {
            get
            {
                if (_routingSlip != null && _routingSlip.ValidationServices != null && _routingSlip.ValidationServices.Count != 0) return _routingSlip;

                _routingSlip = new RoutingSlip {ValidationServices = new List<WorkflowValidationService>()};

                foreach (var validationServiceElement in _paymentValidationServiceConfiguration.ValidationServiceCollection)
                {
                    var routingSlipItem = validationServiceElement as ValidationServiceElement;
                    if (routingSlipItem != null)
                    {
                        _routingSlip.ValidationServices.Add(new WorkflowValidationService()
                        {

                            Endpoint = routingSlipItem.ServiceEndpoint,
                            Name = routingSlipItem.ServiceName,
                            Order = routingSlipItem.SequenceOrder
                        });
                    }
                }
                return _routingSlip;
            }
        }
    }
}