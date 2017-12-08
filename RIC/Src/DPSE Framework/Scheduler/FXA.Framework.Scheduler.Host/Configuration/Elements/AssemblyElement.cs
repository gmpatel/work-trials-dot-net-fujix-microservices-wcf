using System.Configuration;

namespace FXA.Framework.Scheduler.Host.Configuration.Elements
{
    public class AssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
    }
}