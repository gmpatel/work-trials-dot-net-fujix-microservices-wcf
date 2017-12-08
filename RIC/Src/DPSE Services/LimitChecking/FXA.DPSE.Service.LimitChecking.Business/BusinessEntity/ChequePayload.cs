using System;

namespace FXA.DPSE.Service.LimitChecking.Business.BusinessEntity
{
    public class ChequePayload
    {
        public string TrackingId { get; set; }
        public string SessionId { get; set; }
        public string ChannelType { get; set; }

        public DepositCheque[] Cheques { get; set; }
    }
}
