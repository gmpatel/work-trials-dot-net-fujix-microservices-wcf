using System.Configuration;

namespace FXA.DPSE.Service.HealthMonitor.Configuration.Elements
{
    public class EndPointElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }
    }
}