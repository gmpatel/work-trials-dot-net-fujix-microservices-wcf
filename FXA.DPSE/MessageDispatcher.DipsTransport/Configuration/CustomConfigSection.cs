using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.ElementCollections;

namespace FXA.DPSE.MessageDispatcher.DipsTransport.Configuration
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