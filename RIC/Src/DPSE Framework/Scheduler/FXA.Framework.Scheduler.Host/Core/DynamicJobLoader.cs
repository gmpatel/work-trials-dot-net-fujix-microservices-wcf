using System;
using System.Globalization;
using System.Linq;
using Autofac;
using Autofac.Core;
using FXA.DPSE.Framework.Common.Properties;
using FXA.Framework.Scheduler.Host.Properties;
using Quartz;
using Quartz.Impl;
using Topshelf.Quartz;
using Topshelf.ServiceConfigurators;

namespace FXA.Framework.Scheduler.Host.Core
{
    internal static class DynamicJobLoader
    {
        public static void ConfigureBackgroundJobs(IContainer container, ServiceConfigurator<ISchedulerService> service)
        {
            var jobRegistrations =
                container.ComponentRegistry.Registrations.Where(
                    x =>
                        x.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.ComponentType) &&
                        x.Metadata[JobConfigurationMetaDataItemNames.ComponentType].ToString().Equals(typeof (SchedulerJobTemplate).Name, StringComparison.CurrentCultureIgnoreCase)
                    );

            foreach (var jobRegistration in jobRegistrations)
            {
                var componentRegistration = jobRegistration;

                if ((componentRegistration.Services.First() as KeyedService) == null) continue;

                var jobName = ((KeyedService)componentRegistration.Services.First()).ServiceKey.ToString();
                
                var triggerType = componentRegistration.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.TriggerType)
                    ? componentRegistration.Metadata[JobConfigurationMetaDataItemNames.TriggerType].ToString()
                    : JobConfigurationMetaDataItemValues.TriggerTypeSimple.ToString();

                var jobRepeatFrequencyInSeconds = componentRegistration.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.RepeatFrequencyInSeconds)
                    ? int.Parse(componentRegistration.Metadata[JobConfigurationMetaDataItemNames.RepeatFrequencyInSeconds].ToString())
                    : int.Parse(JobConfigurationMetaDataItemValues.RepeatFrequencyInSeconds);

                jobRepeatFrequencyInSeconds = jobRepeatFrequencyInSeconds == 0
                    ? int.Parse(JobConfigurationMetaDataItemValues.RepeatFrequencyInSeconds)
                    : jobRepeatFrequencyInSeconds;

                var cronSchedulePattern = componentRegistration.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.CronSchedulePattern)
                    ? componentRegistration.Metadata[JobConfigurationMetaDataItemNames.CronSchedulePattern].ToString()
                    : JobConfigurationMetaDataItemValues.CronSchedulePattern.ToString();

                var jobRepeatCount = componentRegistration.Metadata.ContainsKey(JobConfigurationMetaDataItemNames.RepeatCount)
                    ? int.Parse(componentRegistration.Metadata[JobConfigurationMetaDataItemNames.RepeatCount].ToString())
                    : int.Parse(JobConfigurationMetaDataItemValues.RepeatCount);

                var componentFullName = componentRegistration.Activator.LimitType.FullName;
                var componentName = componentRegistration.Activator.LimitType.Name;
                var componentType = componentRegistration.Activator.LimitType.Assembly.GetType(componentFullName);

                var trigger = TriggerBuilder.Create()
                        .WithIdentity(string.Format("{0}-Trigger", jobName), null)
                        .WithSchedule(triggerType.Equals(JobConfigurationMetaDataItemValues.TriggerTypeCron, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(cronSchedulePattern)
                            ? (IScheduleBuilder) CronScheduleBuilder.CronSchedule(new CronExpression(cronSchedulePattern))
                            : (jobRepeatCount > 0
                                ? (IScheduleBuilder) SimpleScheduleBuilder.RepeatSecondlyForTotalCount(jobRepeatCount, jobRepeatFrequencyInSeconds)
                                : (IScheduleBuilder) SimpleScheduleBuilder.RepeatSecondlyForever(jobRepeatFrequencyInSeconds)
                              )
                        ).Build();

                service.ScheduleQuartzJob(q =>
                {
                    q.WithJob(() =>
                    {
                        var jobDetail = new JobDetailImpl(jobName, null, componentType);
                        
                        jobDetail.JobDataMap.Put(JobTemplateDataMapKeys.Name, jobName);
                        jobDetail.JobDataMap.Put(JobTemplateDataMapKeys.ComponentName, componentName);
                        jobDetail.JobDataMap.Put(JobTemplateDataMapKeys.ComponentFullName, componentName);

                        foreach (var item in componentRegistration.Metadata)
                        {
                            jobDetail.JobDataMap.Put(item.Key, item.Value);
                        }

                        return jobDetail;
                    });


                    q.AddTrigger(() => trigger);
                });
            }
        }
    }
}
