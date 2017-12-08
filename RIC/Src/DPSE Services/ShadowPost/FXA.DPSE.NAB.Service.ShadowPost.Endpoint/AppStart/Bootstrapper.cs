using System.Data.Entity;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Attributes.Ping;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.ShadowPost;
using FXA.DPSE.Service.ShadowPost.Common.Configuration;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web.Routing;
using FXA.DPSE.Service.ShadowPost.Business;
using FXA.DPSE.Service.ShadowPost.DataAccess;
using FXA.DPSE.Service.ShadowPost.Facade;
using FXA.DPSE.Service.ShadowPost.Facade.Core;

namespace FXA.DPSE.NAB.Service.ShadowPost.Endpoint.AppStart
{
    public class Bootstrapper
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
            var builder = new ContainerBuilder();

            builder.RegisterType<ShadowPostService>().As<IShadowPostService>();
            
            builder.RegisterType<FrameworkConfig>().As<IFrameworkConfig>().SingleInstance();
            builder.RegisterType<ShadowPostServiceConfiguration>().As<IShadowPostServiceConfiguration>().SingleInstance();

            builder.RegisterType<ShadowPostBusiness>().As<IShadowPostBusiness>();

            builder.RegisterType<ShadowPostDataAccess>().As<IShadowPostDataAccess>();
            builder.RegisterType<ShadowPostDb>().As<DbContext>();

            builder.RegisterType<InternetBankingServiceFacade>().As<IInternetBankingServiceFacade>();
            builder.RegisterType<HttpClientProxy>().As<IHttpClientProxy>();

            builder.RegisterModule(new ProxyModule());

            Container = builder.Build();

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
            RouteTable.Routes.Add(new ServiceRoute("dpse", new RestServiceHostFactory<IShadowPostService>(), typeof(IShadowPostService)));
        }
    }
}