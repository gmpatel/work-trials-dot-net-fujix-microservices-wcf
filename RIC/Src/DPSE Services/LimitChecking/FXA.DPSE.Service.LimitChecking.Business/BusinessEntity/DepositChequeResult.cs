namespace FXA.DPSE.Service.LimitChecking.Business.BusinessEntity
{
    public class DepositChequeResult : DepositCheque
    {
        public string ErrorCode { get; set; }
        public string Description { get; set; }

        public DepositChequeResult()
        {
        }
        public DepositChequeResult(string errorCode,string description,string trackingId, int sequenceId, int chequeAmount, string codeline) : 
            base(trackingId, sequenceId, chequeAmount, codeline)
        {
            ErrorCode = errorCode;
            Description = description;
        }
    }
}