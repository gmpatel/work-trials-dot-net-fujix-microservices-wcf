using System.Configuration;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements
{
    public class DestinationElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "Path", IsRequired = true)]
        public DestinationTypes Type
        {
            get { return (DestinationTypes)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }

        [ConfigurationProperty("connection", DefaultValue = null, IsRequired = false)]
        public ConnectionElement Connection
        {
            get { return ((ConnectionElement)(base["connection"])); }
            set { base["connection"] = value; }
        }
    }
}