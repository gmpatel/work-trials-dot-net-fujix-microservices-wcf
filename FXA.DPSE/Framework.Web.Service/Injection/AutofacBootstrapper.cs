using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Configuration;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Defaults;
using FXA.DPSE.Framework.Web.Service.Library.DipsTransport;
using FXA.DPSE.Framework.Web.Service.Library.Processors;

namespace FXA.DPSE.Framework.Web.Service.Injection
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
            
            cb.Register(l => Logger.Instance()).As<ILogger>();

            cb.RegisterType<DipsTrasportProcessor>().As<IDipsTrasportProcessor>();
            cb.RegisterType<DipsTransportService>().As<IDipsTransportService>();
            
            var reader = new ConfigurationSettingsReader();
            cb.RegisterModule(reader);

            Container = cb.Build();

            return Container;
        }
    }
}