using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;

namespace FXA.DPSE.Service.DipsTransport.Business.Entities
{
    public class DipsTransportBusinessInfo : IDpseBusinessEvent
    {
        public DipsTransportBusinessInfo() { }
        public DipsTransportBusinessInfo(string message)
        {
            Message = message;
        }
        
        public string Message { get; private set; }
    }
}