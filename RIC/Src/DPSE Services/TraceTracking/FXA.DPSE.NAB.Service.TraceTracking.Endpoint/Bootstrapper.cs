using System.Data.Entity;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Attributes.Ping;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.TraceTracking;
using FXA.DPSE.Service.TraceTracking.Business;
using FXA.DPSE.Service.TraceTracking.Business.Core;
using FXA.DPSE.Service.TraceTracking.Common.Configuration;
using FXA.DPSE.Service.TraceTracking.DataAccess;
using FXA.DPSE.Service.TraceTracking.DataAccess.Core;

namespace FXA.DPSE.NAB.Service.TraceTracking.Endpoint
{
    public static class Bootstrapper
    {
        public static IContainer Container { get; private set; }

        static Bootstrapper()
        {
            ConfigureContainer();
            ConfigureRoutes();

            ConfigureServiceHost();
        }

        public static IContainer ConfigureContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<TraceTrackingServiceConfiguration>().As<ITraceTrackingServiceConfiguration>().SingleInstance();
            containerBuilder.RegisterType<FrameworkConfig>().As<IFrameworkConfig>().SingleInstance();

            containerBuilder.RegisterType<TraceTrackingDataAccess>().As<ITraceTrackingDataAccess>();
            containerBuilder.RegisterType<TraceTrackingProcessor>().As<ITraceTrackingProcessor>();
            containerBuilder.RegisterType<TraceTrackingService>().As<ITraceTrackingService>();
            containerBuilder.RegisterType<TraceTrackingDb>().As<DbContext>();
            containerBuilder.RegisterModule(new ProxyModule());

            Container = containerBuilder.Build();

            return Container;
        }

        private static void ConfigureServiceHost()
        {
            AutofacHostFactory.HostConfigurationAction = host =>
            {
                foreach (var endpoint in host.Description.Endpoints)
                {
                    var configuredBehaviors = new IEndpointBehavior[endpoint.Behaviors.Count];
                    endpoint.Behaviors.CopyTo(configuredBehaviors, 0);

                    endpoint.Behaviors.Clear();
                    endpoint.Behaviors.Add(new PingEndpointBehavior());

                    foreach (var configuredBehavior in configuredBehaviors)
                    {
                        if (!endpoint.Behaviors.Contains(configuredBehavior.GetType()))
                            endpoint.Behaviors.Add(configuredBehavior);
                    }
                }
            };
        }

        private static void ConfigureRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute(Routes.Dpse, new RestServiceHostFactory<ITraceTrackingService>(), typeof(ITraceTrackingService)));
        }
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
    public static class Routes
    {
        public const string Dpse = "dpse";
    }
}