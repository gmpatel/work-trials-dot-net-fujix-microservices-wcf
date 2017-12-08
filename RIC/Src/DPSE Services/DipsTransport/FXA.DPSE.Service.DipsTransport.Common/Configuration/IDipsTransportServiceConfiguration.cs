using System.Collections.Generic;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration
{
    public interface IDipsTransportServiceConfiguration
    {
        IList<TransportElement> Transports { get; }
        PgpPublicKeyFileElement PgpPublicKeyFile { get; set; }
    }
}