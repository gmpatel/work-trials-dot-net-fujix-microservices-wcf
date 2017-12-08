namespace FXA.DPSE.Service.LimitChecking.Common
{
    public class StatusCode
    {
        public const string LimitCheckSuccessful = "DPSE-4000";
        public const string InputValidationError = "DPSE-4001";
        public const string LimitCheckExceeded = "DPSE-4002";
        public const string DailyTransactionLimitExceeded = "DPSE-4003";
        public const string InternalProcessingError = "DPSE-4004";
    }
}