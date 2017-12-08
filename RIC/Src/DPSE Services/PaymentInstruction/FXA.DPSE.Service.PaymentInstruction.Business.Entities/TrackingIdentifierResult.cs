using System.Collections.Generic;

namespace FXA.DPSE.Service.PaymentInstruction.Business.Entities
{
    public class TrackingIdentifierResult
    {
        public string ForHeader { get; set; }
        public string ForCredit { get; set; }
        public List<PostingChequeTracking> ForCheques { get; set; }
    }

    public class PostingChequeTracking
    {
        public PaymentInstructionPostingCheque Cheque { get; set; }
        public string TrackingId { get; set; }
    }
}