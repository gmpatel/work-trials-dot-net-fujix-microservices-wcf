using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.Elements;
using System.Configuration;

namespace FXA.DPSE.MessageDispatcher.DipsTransport.Configuration
{
    public interface IDipsTransportMessageDispatcherConfig
    {
        IList<EndPointElement> EodTransportEndPoints { get; }
        IList<EndPointElement> PayloadTransportEndPoints { get; }
    }
}