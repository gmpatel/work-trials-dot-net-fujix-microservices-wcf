using System.Configuration;

namespace FXA.DPSE.Framework.Common.Configuration.Elements
{
    public class LoggingServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }

        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
            set { base["enabled"] = value; }
        }
    }
}