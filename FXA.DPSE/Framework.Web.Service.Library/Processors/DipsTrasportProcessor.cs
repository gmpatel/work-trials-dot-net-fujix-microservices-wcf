using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;

namespace FXA.DPSE.Framework.Web.Service.Library.Processors
{
    public class DipsTrasportProcessor : IDipsTrasportProcessor
    {
        public ILogger Logger { get; private set; }
        
        public DipsTrasportProcessor(ILogger logger)
        {
            Logger = logger;
        }

        public bool Process(Guid clientProcessId)
        {
            Logger.Info(string.Format("Begin processing Dips Transport, Client Process Id = {0}", clientProcessId));



            Logger.Info(string.Format("Finished processing Dips Transport, Client Process Id = {0}", clientProcessId));
            
            return true;
        }
    }
}