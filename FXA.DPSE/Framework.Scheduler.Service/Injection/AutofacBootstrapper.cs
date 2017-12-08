using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extras.Quartz;
using FXA.DPSE.Framework.Scheduler.Service.Configuration;
using FXA.DPSE.Framework.Scheduler.Service.Core;
using FXA.DPSE.Framework.Scheduler.Service.Defaults;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Defaults;

namespace FXA.DPSE.Framework.Scheduler.Service.Injection
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

            cb.RegisterType<StandardSchedulerService>().As<ITopshelfService>();
            cb.Register(l => Logger.Instance()).As<ILogger>();

            cb.RegisterModule(new QuartzAutofacFactoryModule());

            cb.RegisterModule(new QuartzAutofacJobsModule(Assembly.GetExecutingAssembly()));
            cb.RegisterModule(new QuartzAutofacJobsModule(typeof(IMessageDispatcherJob).Assembly));
            foreach (var assembly in CustomConfig.Instance.Assemblies)
            {
                cb.RegisterModule(new QuartzAutofacJobsModule(assembly));
            }

            var reader = new ConfigurationSettingsReader();
            cb.RegisterModule(reader);

            Container = cb.Build();

            return Container;
        }
    }
}
