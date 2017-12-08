using System.Collections.Generic;
using Autofac;
using FXA.DPSE.Service.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Core;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;

namespace FXA.DPSE.NAB.Service.PaymentValidation.Endpoint
{
    public class ValidationServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<PaymentValidationMediator>().As<IPaymentValidationMediator>();
            builder.Register(GetValidationComposite).As<IValidationComposite>();
        }

        private IValidationComposite GetValidationComposite(IComponentContext componentContext)
        {
            var validationServices = new Dictionary<string, IValidationHandler>();
            validationServices["Limit"] = componentContext.ResolveKeyed<IValidationHandler>("Limit");
            validationServices["Codeline"] = componentContext.ResolveKeyed<IValidationHandler>("Codeline");
            validationServices["Duplicate"] = componentContext.ResolveKeyed<IValidationHandler>("Duplicate");
            return new ValidationComposite() { Services = validationServices };
        }
    }
}