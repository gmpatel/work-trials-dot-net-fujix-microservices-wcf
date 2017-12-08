using System.Configuration;

namespace FXA.DPSE.Service.Audit.Common.Configuration.Elements
{
    public class ResponseSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("includeErrorStackTrace", DefaultValue = true, IsKey = true, IsRequired = true)]
        public bool IncludeErrorStackTrace
        {
            get { return (bool)base["includeErrorStackTrace"]; }
            set { base["includeErrorStackTrace"] = value; }
        }
    }
}