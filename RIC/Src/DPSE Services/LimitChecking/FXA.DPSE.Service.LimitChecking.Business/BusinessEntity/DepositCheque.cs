namespace FXA.DPSE.Service.LimitChecking.Business.BusinessEntity
{
    public class DepositCheque
    {
        public DepositCheque()
        {
            
        }
        public DepositCheque(string trackingId, int sequenceId, int chequeAmount, string codeline)
        {
            SequenceId = sequenceId;
            ChequeAmount = chequeAmount;
            Codeline = codeline;
            TrackingId = trackingId;
        }

        public string TrackingId { get; set; }
        public int SequenceId { get; set; }
        public int ChequeAmount { get; set; }
        public string Codeline { get; set; }
    }
}
