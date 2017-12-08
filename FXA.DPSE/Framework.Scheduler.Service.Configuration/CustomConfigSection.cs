using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Scheduler.Service.Configuration.ElementCollections;

namespace FXA.DPSE.Framework.Scheduler.Service.Configuration
{
    public class CustomConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("assemblies")]
        public AssemblyElementCollection Assemblies
        {
            get { return ((AssemblyElementCollection)(base["assemblies"])); }
            set { base["assemblies"] = value; }
        }
    }
}