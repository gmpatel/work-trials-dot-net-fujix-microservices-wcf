using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Common;
using FXA.DPSE.Framework.Common.Extensions;
using FXA.DPSE.Framework.Model.DipsTransport;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Properties;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration;
using Quartz;

namespace FXA.DPSE.MessageDispatcher.DipsTransport
{
    public class DipsTransportMessageDispatcherJob : IMessageDispatcherJob
    {
        public ILogger Logger { get; private set; }
        
        public Guid Id { get; private set; }
        
        public DipsTransportMessageDispatcherJob(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            Id = Guid.NewGuid();
            Logger = logger;
        }

        public void Execute(IJobExecutionContext context)
        {
            var name = context.JobDetail.JobDataMap.Get(JobDataMapKeys.Name);

            Logger.Info(string.Format("Job : {0} : {1} : {2} : Executing", this.GetType().Name, name, Id));

            foreach (var ep in CustomConfig.Instance.EndPoints)
            {
                var endPoint = ep;
                var request = new DipsTransportRequest { Id = this.Id, Message = "Process Dips Files Transfer" };

                var thread = new Thread(() =>
                    {
                        Logger.Info(string.Format("Job : {0} : {1} : {2} : Dispatching Request, EndPoint = {3}, Data = {4}", Id, this.GetType().Name, name, endPoint.Url, request.ToJson()));

                        Thread.CurrentThread.IsBackground = true;

                        var response = new RESTClient()
                            .TryPost<DipsTransportRequest, DipsTransportResponse>(endPoint.Url, request)
                            .Result;

                        Logger.Info(string.Format("Job : {0} : {1} : {2} : Response Received, EndPoint = {3}, Data = {4}", Id, this.GetType().Name, name, endPoint.Url, response.ToJson()));
                    }
                );

                thread.Start();
            }

            Logger.Info(string.Format("Job : {0} : {1} : {2} : Completed", this.GetType().Name, name, Id));
        }
    }
}