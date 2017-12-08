using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using FXA.DPSE.Framework.Scheduler.Service.Core;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Properties;
using FXA.DPSE.Framework.Scheduler.Service.Properties;
using Quartz;
using Quartz.Impl;
using Topshelf.Quartz;
using Topshelf.ServiceConfigurators;

namespace FXA.DPSE.Framework.Scheduler.Service.Injection
{
    public static class DynamicJobsInjector
    {
        public static void ConfigureBackgroundJobs(IContainer container, ServiceConfigurator<ITopshelfService> service)
        {
            var jobRegistrations =
                container.ComponentRegistry.Registrations.Where(
                    x =>
                        x.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.ComponentType) &&
                        x.Metadata[JobConfigurationMetaDataItemNames.ComponentType].ToString().Equals(typeof(IMessageDispatcherJob).Name, StringComparison.CurrentCultureIgnoreCase)
                );

            foreach (var jobRegistration in jobRegistrations)
            {
                var componentRegistration = jobRegistration;

                if ((componentRegistration.Services.First() as KeyedService) == null) continue;

                var jobName = ((KeyedService)componentRegistration.Services.First()).ServiceKey.ToString();
                var jobRepeatFrequencyInSeconds = componentRegistration.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.RepeatFrequencyInSeconds)
                    ? int.Parse(componentRegistration.Metadata[JobConfigurationMetaDataItemNames.RepeatFrequencyInSeconds].ToString())
                    : int.Parse(JobConfigurationMetaDataItemValues.RepeatFrequencyInSeconds);
                var jobRepeatCount = componentRegistration.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.RepeatCount)
                    ? int.Parse(componentRegistration.Metadata[JobConfigurationMetaDataItemNames.RepeatCount].ToString())
                    : int.Parse(JobConfigurationMetaDataItemValues.RepeatCount);
                var componentFullName = componentRegistration.Activator.LimitType.FullName;
                var componentName = componentRegistration.Activator.LimitType.Name;
                var componentType = componentRegistration.Activator.LimitType.Assembly.GetType(componentFullName);

                service.ScheduleQuartzJob(q =>
                {
                    q.WithJob(() =>
                    {
                        var jobDetail = new JobDetailImpl(jobName, null, componentType);
                        
                        jobDetail.JobDataMap.Put(JobDataMapKeys.Name, jobName);
                        jobDetail.JobDataMap.Put(JobDataMapKeys.ComponentName, componentName);
                        jobDetail.JobDataMap.Put(JobDataMapKeys.ComponentFullName, componentName);

                        foreach (var item in componentRegistration.Metadata)
                        {
                            jobDetail.JobDataMap.Put(item.Key, item.Value);
                        }

                        return jobDetail;
                    });

                    q.AddTrigger(() => TriggerBuilder.Create()
                        .WithIdentity(string.Format("{0}-Trigger", jobName), null)
                        .WithSchedule(jobRepeatCount > 0 
                            ? SimpleScheduleBuilder.RepeatSecondlyForTotalCount(jobRepeatCount, jobRepeatFrequencyInSeconds) 
                            : SimpleScheduleBuilder.RepeatSecondlyForever(jobRepeatFrequencyInSeconds)
                        )
                        .Build());
                });
            }
        }
    }
}
