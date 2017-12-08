using System;
using Quartz;

namespace FXA.Framework.Scheduler
{
    public abstract class SchedulerJobTemplate : IJob
    {
        public Guid Id { get; private set; }
        
        protected SchedulerJobTemplate()
        {
            Id = Guid.NewGuid();
        }

        public abstract void ExecuteJob(IJobExecutionContext context);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                ExecuteJob(context);
            }
            catch (JobExecutionException exception)
            {
                //if (Logger != null) Logger.Error(string.Format("Something went wrong inside quartz during the execution of job. {0}", exception.Message), exception);
            }
            catch (System.Exception exception)
            {
                //if (Logger != null) Logger.Error(exception.Message, exception);
            }
        }
    }
}