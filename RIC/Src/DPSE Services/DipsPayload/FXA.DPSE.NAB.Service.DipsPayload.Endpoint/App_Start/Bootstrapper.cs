using System.Data.Entity;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Attributes.Ping;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.DipsPayload;
using FXA.DPSE.Service.DipsPayload.Business;
using FXA.DPSE.Service.DipsPayload.Business.Core;
using FXA.DPSE.Service.DipsPayload.Common.Configuration;
using FXA.DPSE.Service.DipsPayload.DataAccess;

namespace FXA.DPSE.NAB.Service.DipsPayload.Endpoint
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

            cb.RegisterType<FrameworkConfig>().As<IFrameworkConfig>().SingleInstance();
            cb.RegisterType<DipsPayloadServiceConfiguration>().As<IDipsPayloadServiceConfiguration>().SingleInstance();

            cb.RegisterType<DipsPayloadService>().As<IDipsPayloadService>();
            
            cb.RegisterType<PaymentInstructionDb>().As<DbContext>();
            cb.RegisterType<PaymentInstructionDataAccess>().As<IPaymentInstructionDataAccess>();

            cb.RegisterModule(new ProxyModule());
            cb.RegisterModule(new BusinessModule());
            
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
            RouteTable.Routes.Add(new ServiceRoute("dpse", new RestServiceHostFactory<IDipsPayloadService>(), typeof(IDipsPayloadService)));
        }
    }
}