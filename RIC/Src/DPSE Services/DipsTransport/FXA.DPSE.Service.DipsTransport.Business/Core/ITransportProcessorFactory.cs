using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Common.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Business.PayloadTransport;
using FXA.DPSE.Service.DipsTransport.Business.SimpleTransport;

namespace FXA.DPSE.Service.DipsTransport.Business.Core
{
    public interface ITransportProcessorFactory
    {
        ITransportProcessor GetPayloadTransporter();
        ITransportProcessor GetEodTransporter();
    }
}