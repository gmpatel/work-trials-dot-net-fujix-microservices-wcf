using System.ServiceModel.Description;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Attributes.Ping;
using FXA.DPSE.Service.DuplicateIdentification;


namespace FXA.DPSE.NAB.Service.DuplicateIdentification.Endpoint
{
    public static class Bootstrapper
    {
        public static IContainer BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<DuplicateIdentificationService>().As<IDuplicateIdentificationService>();
            var container = containerBuilder.Build();
            ConfigureServiceHost();
            return container;
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
    }

}