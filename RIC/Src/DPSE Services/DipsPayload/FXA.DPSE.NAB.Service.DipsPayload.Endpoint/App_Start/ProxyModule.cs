using Autofac;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;

namespace FXA.DPSE.NAB.Service.DipsPayload.Endpoint
{
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