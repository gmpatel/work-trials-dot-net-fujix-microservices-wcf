using System.Configuration;
using FXA.Framework.Scheduler.Host.Configuration.ElementCollections;

namespace FXA.Framework.Scheduler.Host.Configuration
{
    public class SchedulerConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("assemblies")]
        public AssemblyElementCollection Assemblies
        {
            get { return ((AssemblyElementCollection)(base["assemblies"])); }
            set { base["assemblies"] = value; }
        }
    }
}