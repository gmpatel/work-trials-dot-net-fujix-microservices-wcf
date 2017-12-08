namespace FXA.DPSE.Service.PaymentInstruction.Business.Entities
{
    public static class PaymentInstructionStatusCodes
    {
        public const string DupCheque = "DupCheque";
        public const string DailyLimitExceeded = "DailyLimitExceeded";
        public const string TransactionLimitExceeded = "TransactionLimitExceeded";
        public const string AttemptedDuplicateRefNo = "AttemptedDuplicateRefNo";
        public const string ShadowPostFailed = "ShadowPostFailed";
        public const string MalformedRequest = "MalformedRequest";
    }
}