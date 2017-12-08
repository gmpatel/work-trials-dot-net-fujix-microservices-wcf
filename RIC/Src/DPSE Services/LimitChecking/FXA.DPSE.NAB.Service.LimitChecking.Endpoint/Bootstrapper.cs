using Autofac;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.LimitChecking;
using FXA.DPSE.Service.LimitChecking.Business;
using FXA.DPSE.Service.LimitChecking.Common.Configuration;

namespace FXA.DPSE.NAB.Service.LimitChecking.Endpoint
{
    public class Bootstrapper
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LimitCheckingService>().As<ILimitCheckingService>();

            builder.RegisterType<FrameworkConfig>().As<IFrameworkConfig>().SingleInstance();
            builder.RegisterType<LimitCheckingServiceConfiguration>().As<ILimitCheckingServiceConfiguration>().SingleInstance();

            builder.RegisterType<ValidateTransactionLimitBusiness>().As<IValidateTransactionLimitBusiness>();

            builder.RegisterModule(new ProxyModule());

            var container = builder.Build();
            return container;
        }

        public class ProxyModule : Module
        {
            private IAuditProxy FindAuditProxy(IComponentContext context)
            {
                IFrameworkConfig frameworkConfig =
                    context.Resolve<IFrameworkConfig>();

                if (frameworkConfig.Services.AuditService.Enabled)
                {
                    return new AuditProxy(frameworkConfig);
                }
                return new AuditProxyFake();
            }

            private ILoggingProxy FindLoggingProxy(IComponentContext context)
            {
                IFrameworkConfig frameworkConfig =
                    context.Resolve<IFrameworkConfig>();

                if (frameworkConfig.Services.AuditService.Enabled)
                {
                    return new LoggingProxy(frameworkConfig);
                }
                return new LoggingProxyFake();
            }

            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);
                builder.Register(FindAuditProxy).As<IAuditProxy>();
                builder.Register(FindLoggingProxy).As<ILoggingProxy>();

            }
        }
    }
}