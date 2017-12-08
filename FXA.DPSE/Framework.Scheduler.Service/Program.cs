using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Scheduler.Service.Core;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using FXA.DPSE.Framework.Scheduler.Service.Injection;
using Topshelf;
using Topshelf.Autofac;
using FXA.DPSE.Framework.Scheduler.Service.Properties;

namespace FXA.DPSE.Framework.Scheduler.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseAutofacContainer(AutofacBootstrapper.Container);

                x.RunAsLocalSystem();
                x.SetServiceName(Resources.ServiceName);
                x.SetDisplayName(Resources.DisplayName);
                x.SetDescription(Resources.Description);

                x.Service<ITopshelfService>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.Stop());
                    s.WhenPaused(sc => sc.Pause());
                    s.WhenContinued(sc => sc.Continue());

                    DynamicJobsInjector.ConfigureBackgroundJobs(AutofacBootstrapper.Container, s);
                });
            });
        }
    }
}