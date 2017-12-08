using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.ElementCollections;

namespace FXA.DPSE.MessageDispatcher.DipsTransport.Configuration
{
    public class DipsTransportMessageDispatcherConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("payloadTransportEndPoints")]
        public PayloadTransportEndPointElementCollection PayloadTransportEndPoints
        {
            get { return ((PayloadTransportEndPointElementCollection)(base["payloadTransportEndPoints"])); }
            set { base["payloadTransportEndPoints"] = value; }
        }

        [ConfigurationProperty("eodTransportEndPoints")]
        public EodTransportEndPointElementCollection EodTransportEndPoints
        {
            get { return ((EodTransportEndPointElementCollection)(base["eodTransportEndPoints"])); }
            set { base["eodTransportEndPoints"] = value; }
        }
    }
}