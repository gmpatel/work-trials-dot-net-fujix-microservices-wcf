namespace FXA.Framework.Scheduler.Host.Core
{
    internal interface ISchedulerService
    {
        void Start();
        void Stop();
        void Pause();
        void Continue();
    }
}