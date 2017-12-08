using System;
using FXA.Framework.Scheduler.Host.Core;
using FXA.Framework.Scheduler.Host.Properties;
using Topshelf;
using Topshelf.Autofac;

namespace FXA.Framework.Scheduler.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseAutofacContainer(Bootstrapper.Container);

                x.RunAsLocalSystem();
                x.SetServiceName(Resources.ServiceName);
                x.SetDisplayName(Resources.DisplayName);
                x.SetDescription(Resources.Description);

                x.Service<ISchedulerService>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.Stop());
                    s.WhenPaused(sc => sc.Pause());
                    s.WhenContinued(sc => sc.Continue());

                    DynamicJobLoader.ConfigureBackgroundJobs(Bootstrapper.Container, s);
                });
            });

            Console.ReadKey();
        }
    }
}