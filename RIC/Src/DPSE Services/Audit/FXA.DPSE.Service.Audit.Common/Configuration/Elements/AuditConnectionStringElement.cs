using System.Configuration;

namespace FXA.DPSE.Service.Audit.Common.Configuration.Elements
{
    public class AuditConnectionStringElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }
    }
}