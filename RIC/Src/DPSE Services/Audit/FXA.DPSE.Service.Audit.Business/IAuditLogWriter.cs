using Serilog;

namespace FXA.DPSE.Service.Audit.Business
{
    public interface IAuditLogWriter
    {
        void Log(string message, params object[] propertyValues);
    }
}