using Autofac;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;

namespace FXA.DPSE.NAB.Service.ShadowPost.Endpoint.AppStart
{
    public class ProxyModule : Module
    {
        private static IAuditProxy FindAuditProxy(IComponentContext context)
        {
            var frameworkConfig = context.Resolve<IFrameworkConfig>();

            if (frameworkConfig.Services.AuditService.Enabled)
            {
                return new AuditProxy(frameworkConfig);
            }

            return new AuditProxyFake();
        }

        private static ILoggingProxy FindLoggingProxy(IComponentContext context)
        {
            var frameworkConfig = context.Resolve<IFrameworkConfig>();

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