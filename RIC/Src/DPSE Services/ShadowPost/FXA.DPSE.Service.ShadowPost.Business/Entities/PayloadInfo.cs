namespace FXA.DPSE.Service.ShadowPost.Business.Entities
{
    public class PayloadInfo
    {
        public string RequestUtc { get; set; }
        public string RequestGuid { get; set; }
        public string TrackingId { get; set; }
        
        public string ChannelType { get; set; }
        public int ChequeCount { get; set; }

        public string SessionId { get; set; }
        public string DeviceId { get; set; }
        public string IpAddressV4 { get; set; }
    }
}
