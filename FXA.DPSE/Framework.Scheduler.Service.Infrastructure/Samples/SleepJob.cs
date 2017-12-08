using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using Quartz;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Properties;

namespace FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Samples
{
    public class SleepJob : IMessageDispatcherJob
    {
        public ILogger Logger { get; private set; }
        public Guid Id { get; private set; }
        public IEnumerable<IMessageDispatcherTask> Tasks { get; set; }

        public SleepJob(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            Id = Guid.NewGuid();
        
            Logger = logger;
        }

        public void Execute(IJobExecutionContext context)
        {
            var sleepInterval = 5000;
            int.TryParse((context.JobDetail.JobDataMap.ContainsKey(JobDataMapKeys.SleepIntervalInMiliseconds) ? context.JobDetail.JobDataMap.Get(JobDataMapKeys.SleepIntervalInMiliseconds).ToString() : "5000"), out sleepInterval);
            
            var name = context.JobDetail.JobDataMap.Get(JobDataMapKeys.Name);
            
            Logger.Info(string.Format("Job : {0} : {1} : {2} : Executing", this.GetType().Name, name, Id));
    
            Logger.Info(string.Format("Job : {0} : {1} : {2} : Going To Sleep For {3} Miliseconds - {3}", Id, this.GetType().Name, name, sleepInterval));

            Thread.Sleep(sleepInterval);

            Logger.Info(string.Format("Job : {0} : {1} : {2} : Sleep For {3} Miliseconds Performed - {3}", Id, this.GetType().Name, name, sleepInterval));

            Logger.Info(string.Format("Job : {0} : {1} : {2} : Completed", this.GetType().Name, name, Id));
        }
    }
}   