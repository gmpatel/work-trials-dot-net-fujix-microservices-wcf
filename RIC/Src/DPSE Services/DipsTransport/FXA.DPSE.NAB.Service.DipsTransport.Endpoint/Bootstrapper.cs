using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Attributes.Ping;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DipsTransport;
using FXA.DPSE.Service.DipsTransport.Business;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.EodTransport;
using FXA.DPSE.Service.DipsTransport.Common.Configuration;
using FXA.DPSE.Service.DipsTransport.Business.PayloadTransport;
using FXA.DPSE.Service.DipsTransport.Business.SimpleTransport;
using FXA.DPSE.Service.DipsTransport.DataAccess;

namespace FXA.DPSE.NAB.Service.DipsTransport.Endpoint
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
            cb.RegisterType<DipsTransportServiceConfiguration>().As<IDipsTransportServiceConfiguration>().SingleInstance();
            cb.RegisterType<DipsTransportService>().As<IDipsTransportService>();
            cb.RegisterType<DipsTransportMetadataSerializer>().As<IDipsTransportMetadataSerializer>();
            cb.RegisterType<DipsTransportMetadataCreator>().As<IDipsTransportMetadataCreator>();
            cb.RegisterType<DipsTransportZipCreator>().As<IDipsTransportZipCreator>();
            cb.RegisterType<DipsTransportPgpCreator>().As<IDipsTransportPgpCreator>();
            cb.RegisterType<FileSystem>().As<IFileSystem>();

            cb.RegisterType<TransportProcessorFactory>().As<ITransportProcessorFactory>();
            
            cb.RegisterType<PathToPathTransportProcessor>().Named<ITransportProcessor>("PathToPathTransportProcessor");
            cb.RegisterType<PathToSftpTransportProcessor>().Named<ITransportProcessor>("PathToSftpTransportProcessor");
            cb.RegisterType<SftpToPathTransportProcessor>().Named<ITransportProcessor>("SftpToPathTransportProcessor");
            cb.RegisterType<SftpToSftpTransportProcessor>().Named<ITransportProcessor>("SftpToSftpTransportProcessor");

            cb.RegisterType<DefaultPayloadTransportProcessor>().Named<ITransportProcessor>("DefaultPayloadTransportProcessor");
            cb.RegisterType<DefaultEodTransportProcessor>().Named<ITransportProcessor>("DefaultEodTransportProcessor");

            cb.RegisterType<PaymentInstructionDb>().Named<DbContext>("PaymentInstructionDb");
            cb.RegisterType<DipsTransportDb>().Named<DbContext>("DipsTransportDb");

            cb.RegisterModule(new TransportDynamicAutofacModule());

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
            RouteTable.Routes.Add(new ServiceRoute(Routes.Dpse, new RestServiceHostFactory<IDipsTransportService>(), typeof(IDipsTransportService)));
        }
    }

    public class TransportDynamicAutofacModule : Module
    {
        private static IDipsTransportDataAccess FindDipsTransportDataAccess(IComponentContext context)
        {
            var paymentInstructionDb = context.ResolveNamed<DbContext>("PaymentInstructionDb");
            var dipsTransportDb = context.ResolveNamed<DbContext>("DipsTransportDb");
            var auditProxy = context.Resolve<IAuditProxy>();

            return new DipsTransportDataAccess(paymentInstructionDb, dipsTransportDb, auditProxy);
        }

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
            builder.Register(FindDipsTransportDataAccess).As<IDipsTransportDataAccess>();
        }
    }

    public static class Routes
    {
        public const string Dpse = "dpse";
    }
}