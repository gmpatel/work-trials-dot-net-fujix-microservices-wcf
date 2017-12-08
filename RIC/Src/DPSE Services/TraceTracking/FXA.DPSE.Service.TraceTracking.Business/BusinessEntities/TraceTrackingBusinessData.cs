using System;

namespace FXA.DPSE.Service.TraceTracking.Business.BusinessEntities
{
    public class TraceTrackingBusinessData
    {
        public string RequestId { get; set; }
        public string ChannelType { get; set; }
        public TraceTrackingSession ClientSession { get; set; }
        public int ChequeCount { get; set; }
        public int TotalTransactionAmount { get; set; }
        public TraceTrackingAccountDetails DepositingAccountDetails { get; set; }
    }

    public class TraceTrackingSession
    {
        public string SessionId { get; set; }
        public string DeviceId { get; set; }
        public string IpAddressV4 { get; set; }
    }

    public class TraceTrackingAccountDetails
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BsbCode { get; set; }
        public string AccountType { get; set; }
    }
}