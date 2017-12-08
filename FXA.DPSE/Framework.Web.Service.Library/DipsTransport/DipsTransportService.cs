using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FXA.DPSE.Framework.Common.Extensions;
using FXA.DPSE.Framework.Model.DipsTransport;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using System.Threading;
using FXA.DPSE.Framework.Web.Service.Library.Processors;

namespace FXA.DPSE.Framework.Web.Service.Library.DipsTransport
{
    public class DipsTransportService : IDipsTransportService
    {
        public ILogger Logger { get; private set; }
        public IDipsTrasportProcessor TransportProcessor { get; private set; }

        public DipsTransportService(ILogger logger, IDipsTrasportProcessor processor)
        {
            Logger = logger;
            TransportProcessor = processor;
        }

        public DipsTransportResponse Post(DipsTransportRequest request)
        {
            Logger.Info(string.Format("Request Received, Client Process Id = {0}, Data = {1}", request.Id, request.ToJson()));

            var thread = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    TransportProcessor.Process(request.Id);
                }
            );

            thread.Start();

            Logger.Info(string.Format("Returning Response, Client Process Id = {0}", request.Id));

            return new DipsTransportResponse {Id = request.Id};
        }
    }
}