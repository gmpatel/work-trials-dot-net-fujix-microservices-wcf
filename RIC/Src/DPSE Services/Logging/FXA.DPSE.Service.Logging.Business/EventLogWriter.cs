using System;
using log4net.Config;
using Serilog;

namespace FXA.DPSE.Service.Logging.Business
{
    //TODO: https://github.com/nblumhardt/autofac-serilog-integration

    public class EventLogWriter : IEventLogWriter
    {
        public void Log(string message, string machineName, params object[] propertyValues)
        {
            log4net.GlobalContext.Properties["machineName"] = machineName;
            log4net.GlobalContext.Properties["date"] = DateTime.Now.ToString("yyyy-MMMM-dd");

            var logger = new LoggerConfiguration()
                .MinimumLevel
                .Debug()
                .WriteTo
                .Log4Net()
                .CreateLogger();
            XmlConfigurator.Configure();
            logger.Information(message, propertyValues);
        }
    }
}