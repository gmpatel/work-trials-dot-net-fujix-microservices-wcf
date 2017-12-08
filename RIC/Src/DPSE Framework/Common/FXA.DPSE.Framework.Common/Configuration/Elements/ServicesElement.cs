using System.Configuration;

namespace FXA.DPSE.Framework.Common.Configuration.Elements
{
    public class ServicesElement : ConfigurationElement
    {
        [ConfigurationProperty("loggingService", DefaultValue = null, IsRequired = true)]
        public LoggingServiceElement LoggingService
        {
            get { return ((LoggingServiceElement)(base["loggingService"])); }
            set { base["loggingService"] = value; }
        }

        [ConfigurationProperty("auditService", DefaultValue = null, IsRequired = true)]
        public AuditServiceElement AuditService
        {
            get { return ((AuditServiceElement)(base["auditService"])); }
            set { base["auditService"] = value; }
        }
    }
}