using System.Configuration;

namespace FXA.DPSE.Service.DipsPayload.Common.Configuration.Elements
{
    public class PayloadFileSystemLocationElement : ConfigurationElement
    {
        [ConfigurationProperty("path", DefaultValue = null, IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }
    }
}