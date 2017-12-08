using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core
{
    public interface IMessageDispatcherTask
    {
        Guid Id { get; }
        string Name { get; set; }

        object Process();
    }
}