using Autofac;
using FXA.DPSE.Service.Logging;
using FXA.DPSE.Service.Logging.Business;

namespace FXA.DPSE.NAB.Service.Logging.Endpoint
{
    public class Bootstrapper
    {
        public static IContainer BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<LoggingService>().As<ILoggingService>();
            builder.RegisterType<LoggingBusiness>().As<ILoggingBusiness>();
            builder.RegisterType<EventLogWriter>().As<IEventLogWriter>();
            
            IContainer container = builder.Build();
            return container;
        }
    }
}
