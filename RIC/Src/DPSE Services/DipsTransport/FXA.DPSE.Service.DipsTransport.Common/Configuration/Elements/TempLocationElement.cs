using System.Configuration;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements
{
    public class TempLocationElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "Path", IsRequired = true)]
        public TempLocationTypes Type
        {
            get { return (TempLocationTypes)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }
    }
}