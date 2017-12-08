using System.Data.Entity;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Attributes.Ping;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.PaymentInstruction;
using FXA.DPSE.Service.PaymentInstruction.Business;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.DataAccess;

namespace FXA.DPSE.NAB.Service.PaymentInstruction.Endpoint
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

            cb.RegisterType<PaymentInstructionServiceConfiguration>().As<IPaymentInstructionServiceConfiguration>().SingleInstance();
            cb.RegisterType<FrameworkConfig>().As<IFrameworkConfig>().SingleInstance();

            cb.RegisterType<PaymentInstructionService>().As<IPaymentInstructionService>();
            cb.RegisterType<PaymentInstructionBusiness>().As<IPaymentInstructionBusiness>();
            
            cb.RegisterType<PaymentInstructionDataAccess>().As<IPaymentInstructionDataAccess>();
            
            //TODO: Correct the following when Framework.DataAcccess impl completed !
            cb.RegisterType<PaymentInstructionDb>().As<DbContext>();

            cb.RegisterModule(new ProxyModule());
            cb.RegisterModule(new FacadeModule());
            
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
            RouteTable.Routes.Add(new ServiceRoute("dpse", new RestServiceHostFactory<IPaymentInstructionService>(), typeof(IPaymentInstructionService)));
        }
    }
}