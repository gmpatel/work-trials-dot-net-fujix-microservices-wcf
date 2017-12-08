namespace FXA.DPSE.Service.DipsPayload.Common
{ 
    public class StatusCode
    {
        public const string DipsPayloadCreationSuccessful = "DPSE-6000";
        public const string InputValidationError = "DPSE-6001";
        public const string DatabaseOrFileAccessError = "DPSE-6002";
        public const string InternalProcessingError = "DPSE-6003";
        public const string PaymentInstructionNotFound = "DPSE-6004";
    }
}
