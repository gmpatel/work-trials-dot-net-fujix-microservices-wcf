using System;

namespace FXA.Framework.Scheduler
{
    internal interface ISchedulerTask
    {
        Guid Id { get; }
        string Name { get; set; }

        object Process();
    }
}