namespace FXA.DPSE.Service.PaymentValidation.Common
{
    public class ResponseCode
    {
        //according to v0.4 documentation for logging service

        public const string ValidationSuccessful = "DPSE-8000";
        public const string ValidationFailed = "DPSE-8001";
        public const string InputValidationError = "DPSE-8002";
        public const string InternalProcessingError = "DPSE-8003";
        public const string DatabaseOrFileAccessError = "DPSE-8004";
    }

    public class ValidationStatus
    {
        public const string ValidationSuccessful = "Success";
        public const string ValidationFailure = "Fail";

    }   
}