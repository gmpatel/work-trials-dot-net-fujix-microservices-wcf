using Autofac;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Service.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Common.Configuration;
using FXA.DPSE.Service.PaymentValidation.Core;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;

namespace FXA.DPSE.NAB.Service.PaymentValidation.Endpoint
{
    public class Bootstrapper
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PaymentValidationService>().As<IPaymentValidationService>();

            builder.RegisterType<FrameworkConfig>().As<IFrameworkConfig>().SingleInstance();
            builder.RegisterType<PaymentValidationServiceConfiguration>().As<IPaymentValidationServiceConfiguration>().SingleInstance();

            builder.RegisterType<LimitValidationHandler>().As<IValidationHandler>().Keyed<IValidationHandler>("Limit");
            builder.RegisterType<CodelineValidationHandler>().As<IValidationHandler>().Keyed<IValidationHandler>("Codeline");
            builder.RegisterType<DuplicateValidationHandler>().As<IValidationHandler>().Keyed<IValidationHandler>("Duplicate");

            builder.RegisterModule(new ValidationServiceModule());
            builder.RegisterModule(new ProxyModule());

            var container = builder.Build();
            return container;
        }
    }
}