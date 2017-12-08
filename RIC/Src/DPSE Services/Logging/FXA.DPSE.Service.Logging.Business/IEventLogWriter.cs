namespace FXA.DPSE.Service.Logging.Business
{
    public interface IEventLogWriter
    {
        void Log(string message, string machineName, params object[] propertyValues);
    }
}