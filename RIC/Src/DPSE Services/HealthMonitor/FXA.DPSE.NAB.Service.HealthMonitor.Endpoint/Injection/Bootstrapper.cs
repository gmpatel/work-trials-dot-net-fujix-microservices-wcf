using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Attributes.Ping;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.HealthMonitor;
using FXA.DPSE.Service.HealthMonitor.Configuration;

namespace FXA.DPSE.NAB.Service.HealthMonitor.Endpoint.Injection
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
            var cb = new ContainerBuilder();

            cb.RegisterType<CustomConfig>().As<ICustomConfig>().SingleInstance();
            cb.RegisterType<HealthMonitorService>().As<IHealthMonitorService>();

            Container = cb.Build();

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
            RouteTable.Routes.Add(new ServiceRoute(Routes.Dpse, new RestServiceHostFactory<IHealthMonitorService>(), typeof(IHealthMonitorService)));
            RouteTable.Routes.Add(new ServiceRoute(Routes.HealthMonitor, new RestServiceHostFactory<IHealthMonitorService>(), typeof(IHealthMonitorService)));
            RouteTable.Routes.Add(new ServiceRoute(Routes.HealthMonitorService, new RestServiceHostFactory<IHealthMonitorService>(), typeof(IHealthMonitorService)));
            RouteTable.Routes.Add(new ServiceRoute(Routes.DpseHealthMonitor, new RestServiceHostFactory<IHealthMonitorService>(), typeof(IHealthMonitorService)));
            RouteTable.Routes.Add(new ServiceRoute(Routes.DpseHealthMonitorService, new RestServiceHostFactory<IHealthMonitorService>(), typeof(IHealthMonitorService)));
        }
    }

    public static class Routes
    {
        public const string Dpse = "dpse"; 
        public const string HealthMonitor = "healthmonitor";
        public const string HealthMonitorService = "healthmonitorservice";
        public const string DpseHealthMonitor = "dpse/healthmonitor";
        public const string DpseHealthMonitorService = "dpse/healthmonitorservice";
    }
}