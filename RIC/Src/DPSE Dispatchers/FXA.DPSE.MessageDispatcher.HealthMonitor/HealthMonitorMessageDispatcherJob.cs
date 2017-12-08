using System.Threading;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.MessageDispatcher.HealthMonitor.Configuration;
using FXA.DPSE.Service.DTO.HealthMonitor;
using FXA.Framework.Scheduler;
using Quartz;

namespace FXA.DPSE.MessageDispatcher.HealthMonitor
{
    public class HealthMonitorMessageDispatcherJob : SchedulerJobTemplate
    {
        public HealthMonitorMessageDispatcherJob() : base()
        {
        }

        public override void ExecuteJob(IJobExecutionContext context)
        {
            //var name = context.JobDetail.JobDataMap.Get(JobTemplateDataMapKeys.Name);
            //TODO: Call LoggingService instead.
            //Logger.Instance().Info(string.Format("Job : {0} : {1} : {2} : Executing", this.GetType().Name, name, Id));

            foreach (var ep in CustomConfig.Instance.EndPoints)
            {
                var endPoint = ep;
                var request = new HealthMonitorPostRequest{ Id = this.Id, Message = "Process Health Check" };

                var thread = new Thread(() =>
                    {
                        //TODO: Call LoggingService instead.
                        //Logger.Instance().Info(string.Format("Job : {0} : {1} : {2} : Dispatching Request, EndPoint = {3}, Data = {4}", Id, this.GetType().Name, name, endPoint.Url, request.ToJson()));

                        Thread.CurrentThread.IsBackground = true;

                        var response = HttpClientExtensions
                            .PostAsyncAsJson<HealthMonitorPostRequest, HealthMonitorPostResponse>(endPoint.Url, request)
                            .Result;

                        //TODO: Call LoggingService instead.
                        //Logger.Instance().Info(string.Format("Job : {0} : {1} : {2} : Response Received, EndPoint = {3}, Data = {4}", Id, this.GetType().Name, name, endPoint.Url, response.ToJson()));
                    }
                );

                thread.Start();
            }
            //TODO: Call LoggingService instead.
            //Logger.Instance().Info(string.Format("Job : {0} : {1} : {2} : Completed", this.GetType().Name, name, Id));
        }
    }
}