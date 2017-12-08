using Autofac;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Serilog.Sinks.MSSqlServer;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.Audit;
using FXA.DPSE.Service.Audit.Business;
using FXA.DPSE.Service.Audit.Common.Configuration;
using FXA.DPSE.Service.Audit.DataAccess;
using Serilog;
using System.Data.Entity;

namespace FXA.DPSE.NAB.Service.Audit.Endpoint
{
    public class Bootstrapper
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AuditService>().As<IAuditService>();
            builder.RegisterType<AuditBusiness>().As<IAuditBusiness>();
            //builder.RegisterType<AuditLogWriter>().As<IAuditLogWriter>();
            builder.RegisterType<FrameworkConfig>().As<IFrameworkConfig>();
            builder.RegisterType<AuditCustomConfig>().As<IAuditCustomConfig>();
            builder.RegisterType<AuditDataAccess>().As<IAuditDataAccess>();
            builder.RegisterType<AuditDb>().As<DbContext>();

            builder.RegisterModule(new ProxyModule());
            //builder.RegisterModule(new LogModule()); 
    
            var container = builder.Build();
            
            return container;
        }

        public class LogModule : Module
        {
            //private static ILogger FindLogger(IComponentContext context)
            //{
            //    var serviceConfig = context.Resolve<IAuditCustomConfig>();

            //    var connectionString = serviceConfig.AuditConnectionStringElement.Value;
            //    var tableName = serviceConfig.TableNameElement.Value;

            //    var logger = new LoggerConfiguration()
            //        .WriteTo
            //        .MSSqlServer(connectionString, tableName)
            //        .CreateLogger();

            //    return logger;
            //}

            //protected override void Load(ContainerBuilder builder)
            //{
            //    base.Load(builder);
            //    builder.Register(FindLogger).As<ILogger>();
            //}
        }

        public class ProxyModule : Module
        {
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
                builder.Register(FindLoggingProxy).As<ILoggingProxy>();
            }
        }
    }
}