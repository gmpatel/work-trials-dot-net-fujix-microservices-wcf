using System.Configuration;
using FXA.DPSE.Service.Audit.Common.Configuration.Elements;

namespace FXA.DPSE.Service.Audit.Common.Configuration.Section
{
    public class CustomConfigSection : ConfigurationSection
    {
        //[ConfigurationProperty("auditConnectionString")]
        //public AuditConnectionStringElement AuditConnectionStringElement
        //{
        //    get { return ((AuditConnectionStringElement)(base["auditConnectionString"])); }
        //    set { base["auditConnectionString"] = value; }
        //}

        //[ConfigurationProperty("tableName")]
        //public TableNameElement TableName
        //{
        //    get { return ((TableNameElement)(base["tableName"])); }
        //    set { base["tableName"] = value; }
        //}

        [ConfigurationProperty("responseSettings")]
        public ResponseSettingsElement ResponseSettings
        {
            get { return ((ResponseSettingsElement)(base["responseSettings"])); }
            set { base["responseSettings"] = value; }
        }
    }
}