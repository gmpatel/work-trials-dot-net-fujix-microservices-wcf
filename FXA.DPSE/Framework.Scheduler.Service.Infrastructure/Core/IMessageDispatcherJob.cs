using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core
{
    public interface IMessageDispatcherJob : IJob
    {
        Guid Id { get; }
    }
}