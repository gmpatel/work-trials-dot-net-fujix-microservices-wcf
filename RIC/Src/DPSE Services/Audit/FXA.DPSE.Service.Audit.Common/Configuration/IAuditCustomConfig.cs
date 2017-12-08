using FXA.DPSE.Service.Audit.Common.Configuration.Elements;

namespace FXA.DPSE.Service.Audit.Common.Configuration
{
    public interface IAuditCustomConfig
    {
        //AuditConnectionStringElement AuditConnectionStringElement { get; }
        //TableNameElement TableNameElement { get; }

        ResponseSettingsElement ResponseSettingsElement { get; }
    }
}