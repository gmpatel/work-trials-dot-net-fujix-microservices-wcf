using FXA.DPSE.Framework.Service.WCF.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsTransport.Business
{
    public abstract class BaseTransportProcessor : ITransportProcessor
    {
        public TransportElement Configuration { get; set; }
        public SourceElement Source { get; set; }
        public DestinationElement Destination { get; set; }
        public TempLocationElement TempLocation { get; set; }

        protected BaseTransportProcessor(TransportElement transport)
        {
            Configuration = transport;
            Source = transport != null ? transport.Source : null;
            Destination = transport != null ? transport.Destination : null;
            TempLocation = transport != null ? transport.TempLocation : null;
        }

        public abstract DipsTransportBusinessResult Transport(DipsTransportBusinessData data);
    }
}