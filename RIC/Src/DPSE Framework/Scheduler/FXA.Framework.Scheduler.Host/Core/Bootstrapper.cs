using System.Reflection;
using Autofac;
using Autofac.Configuration;
using Autofac.Extras.Quartz;
using FXA.Framework.Scheduler.Host.Configuration;

namespace FXA.Framework.Scheduler.Host.Core
{
    internal static class Bootstrapper
    {
        public static IContainer Container { get; private set; }

        static Bootstrapper()
        {
            ConfigureContainer();
        }

        public static IContainer ConfigureContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<SchedulerService>().As<ISchedulerService>();

            //TODO: You need to register the logger as SingleInstance() by Autofac.
            //containerBuilder.Register(l => Logger.Instance()).As<ILogger>();

            containerBuilder.RegisterModule(new QuartzAutofacFactoryModule());

            containerBuilder.RegisterModule(new QuartzAutofacJobsModule(Assembly.GetExecutingAssembly()));
            containerBuilder.RegisterModule(new QuartzAutofacJobsModule(typeof(SchedulerJobTemplate).Assembly));

            foreach (var assembly in SchedulerConfig.Instance.Assemblies)
            {
                containerBuilder.RegisterModule(new QuartzAutofacJobsModule(assembly));
            }

            var configurationSettingReader = new ConfigurationSettingsReader();
            containerBuilder.RegisterModule(configurationSettingReader);

            Container = containerBuilder.Build();

            return Container;
        }
    }
}
