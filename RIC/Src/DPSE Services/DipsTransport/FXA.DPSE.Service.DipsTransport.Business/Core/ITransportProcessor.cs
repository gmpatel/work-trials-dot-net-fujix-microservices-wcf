using System;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Service.DipsTransport.Business.Entities;

namespace FXA.DPSE.Service.DipsTransport.Business.Core
{
    public interface ITransportProcessor
    {
        DipsTransportBusinessResult Transport(DipsTransportBusinessData data);
    }
}