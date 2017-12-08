using System.Configuration;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements
{
    public class SourceElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "Path", IsRequired = true)]
        public SourceTypes Type
        {
            get { return (SourceTypes)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }

        [ConfigurationProperty("regex", DefaultValue = "", IsRequired = true)]
        public string RegEx
        {
            get { return (string)base["regex"]; }
            set { base["regex"] = value; }
        }

        [ConfigurationProperty("connection", DefaultValue = null, IsRequired = false)]
        public ConnectionElement Connection
        {
            get { return ((ConnectionElement)(base["connection"])); }
            set { base["connection"] = value; }
        }
    }
}