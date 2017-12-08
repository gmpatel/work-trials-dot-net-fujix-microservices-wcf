using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Wcf;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Errors;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Ping;
using FXA.DPSET.Framework.Service.Library;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace FXA.DPSET.Framework.Service.Injection
{
    public static class AutofacBootstrapper
    {
        public static IContainer Container { get; private set; }

        static AutofacBootstrapper()
        {
            ConfigureContainer();
        }
        
        public static IContainer ConfigureContainer()
        {
            var cb = new ContainerBuilder();

            cb.RegisterType<HealthMonitorService>().As<IHealthMonitorService>();

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

            var reader = new ConfigurationSettingsReader();
            cb.RegisterModule(reader);

            Container = cb.Build();

            return Container;
        }
    }
}