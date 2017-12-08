using System.Configuration;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements
{
    public class PgpPublicKeyFileElement : ConfigurationElement
    {
        [ConfigurationProperty("path", DefaultValue = null, IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }
    }
}