using System;
using Quartz;

namespace FXA.Framework.Scheduler
{
    public interface IMessageDispatcherJob : IJob
    {
        Guid Id { get; }
    }
}