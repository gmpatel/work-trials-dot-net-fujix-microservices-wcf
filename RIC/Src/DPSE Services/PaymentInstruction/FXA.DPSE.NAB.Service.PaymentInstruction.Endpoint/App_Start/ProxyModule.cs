using Autofac;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.NAB.Service.PaymentInstruction.Endpoint
{
    public class ProxyModule : Module
    {
        private IAuditProxy FindAuditProxy(IComponentContext context)
        {
            var frameworkConfig = context.Resolve<IFrameworkConfig>();
            return frameworkConfig.Services.AuditService.Enabled ? (IAuditProxy)new AuditProxy(frameworkConfig) : new AuditProxyFake();
        }

        private ILoggingProxy FindLoggingProxy(IComponentContext context)
        {
            var frameworkConfig = context.Resolve<IFrameworkConfig>();
            return frameworkConfig.Services.AuditService.Enabled ? (ILoggingProxy)new LoggingProxy(frameworkConfig) : new LoggingProxyFake();
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<HttpClientProxy>().As<IHttpClientProxy>();

            builder.Register(FindAuditProxy).As<IAuditProxy>();
            builder.Register(FindLoggingProxy).As<ILoggingProxy>();

        }
    }
}