using FXA.Framework.Scheduler;
using System;
using System.Threading;
using FXA.DPSE.Framework.Common.Properties;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration;
using FXA.DPSE.Service.DTO.DipsTransport;
using Quartz;

namespace FXA.DPSE.MessageDispatcher.DipsTransport
{
    public class DipsTransportEodMessageDispatcherJob : SchedulerJobTemplate
    {
        private readonly IDipsTransportMessageDispatcherConfig _dispatcherConfig;

        public DipsTransportEodMessageDispatcherJob(IDipsTransportMessageDispatcherConfig dispatcherConfig)
        {
            _dispatcherConfig = dispatcherConfig;
        }

        public override void ExecuteJob(IJobExecutionContext context)
        {
            var name = context.JobDetail.JobDataMap.Get(JobTemplateDataMapKeys.Name);

            //TODO: Call LoggingService instead.
            //Logger.Instance().Info(string.Format("Job : {0} : {1} : {2} : Executing", this.GetType().Name, name, Id));

            foreach (var ep in _dispatcherConfig.EodTransportEndPoints)
            {
                var endPoint = ep;

                var request = new DipsTransportEodRequest
                {
                    MessageVersion = "2.0",
                    RequestGuid = Id.ToString(),
                    RequestUtc = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss").Replace(" ", "T"),
                    ReportDate = DateTime.UtcNow.Date.ToString("yyyy-MM-dd"),
                    IpAddressV4 = "127.0.0.1"
                };

                var thread = new Thread(() =>
                    {
                        //TODO: Call LoggingService instead.
                        //Logger.Instance().Info(string.Format("Job : {0} : {1} : {2} : Dispatching Request, EndPoint = {3}, Data = {4}", Id, this.GetType().Name, name, endPoint.Url, request.ToJson()));

                        Console.WriteLine(string.Format("{3} - Executing Job, Id = {2}, Name = {1}, For Endpoint {0}", endPoint.Url, name, Id, DateTime.Now.ToString("HH:mm:ss")));
                        
                        Thread.CurrentThread.IsBackground = true;
                        
                        try
                        {
                            var response = HttpClientExtensions
                                .PostAsyncAsJson<DipsTransportEodRequest, DipsTransportEodResponse>(endPoint.Url,
                                    request)
                                .Result;
                        
                            Console.WriteLine(string.Format("{4} - Finished Job, Id = {2}, Name = {1}, For Endpoint {0}, Response = {3}, Message = {5}", endPoint.Url, name, Id, response.StatusCode, DateTime.Now.ToString("HH:mm:ss"), response.Content.Message));
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(string.Format("{4} - Finished Job (With Exception), Id = {2}, Name = {1}, For Endpoint {0}, Exception = {3}, Message = {5}", endPoint.Url, name, Id, exception.GetType().Name, DateTime.Now.ToString("HH:mm:ss"), exception.Message));
                        }
                        
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