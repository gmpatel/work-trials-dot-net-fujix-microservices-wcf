using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Framework.Scheduler.Service.Core
{
    public interface ITopshelfService
    {
        void Start();
        void Stop();
        void Pause();
        void Continue();
    }
}