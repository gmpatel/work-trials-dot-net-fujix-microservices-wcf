using System.Configuration;
using FXA.DPSE.Service.Audit.Common.Configuration.Elements;
using FXA.DPSE.Service.Audit.Common.Configuration.Section;

namespace FXA.DPSE.Service.Audit.Common.Configuration
{
    public class AuditCustomConfig : IAuditCustomConfig
    {
        private readonly CustomConfigSection _config;

        //private AuditConnectionStringElement _auditConnectionStringElement;
        //private TableNameElement _tableNameElement;
        private ResponseSettingsElement _responseSettingsElement;

        public AuditCustomConfig()
        {
            _config = (CustomConfigSection)ConfigurationManager.GetSection("serviceConfig");
        }

        //public AuditConnectionStringElement AuditConnectionStringElement
        //{
        //    get
        //    {
        //        if (_auditConnectionStringElement == null)
        //        {
        //            _auditConnectionStringElement = _config.AuditConnectionStringElement;
        //        }
        //        return _auditConnectionStringElement;
        //    }
        //}

        //public TableNameElement TableNameElement
        //{
        //    get
        //    {
        //        if (_tableNameElement == null)
        //        {
        //            _tableNameElement = _config.TableName;
        //        }
        //        return _tableNameElement;
        //    }
        //}

        public ResponseSettingsElement ResponseSettingsElement
        {
            get
            {
                if (_responseSettingsElement == null)
                {
                    _responseSettingsElement = _config.ResponseSettings;
                }

                return _responseSettingsElement;
            }
        }
    }
}