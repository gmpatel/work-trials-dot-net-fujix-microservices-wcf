using System.Configuration;
using FXA.DPSE.Service.HealthMonitor.Configuration.ElementCollections;

namespace FXA.DPSE.Service.HealthMonitor.Configuration.Section
{
    public class CustomConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("endPoints")]
        public EndPointElementCollection EndPoints
        {
            get { return ((EndPointElementCollection)(base["endPoints"])); }
            set { base["endPoints"] = value; }
        }
    }
}