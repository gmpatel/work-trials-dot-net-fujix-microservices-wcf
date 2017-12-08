using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.Elements
{
    public class EndPointElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }
    }
}