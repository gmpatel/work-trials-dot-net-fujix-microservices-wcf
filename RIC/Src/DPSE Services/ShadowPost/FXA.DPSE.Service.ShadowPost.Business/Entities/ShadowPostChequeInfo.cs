namespace FXA.DPSE.Service.ShadowPost.Business.Entities
{
    public class ShadowPostedChequeInfo
    {
        public string TrackingId { get; set; }
        public int SequenceId { get; set; }
        public string Codeline { get; set; }
        public string SettlementDate { get; set; }
    }
}