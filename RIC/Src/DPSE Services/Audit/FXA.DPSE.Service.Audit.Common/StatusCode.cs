namespace FXA.DPSE.Service.Audit.Common
{
    public class StatusCode
    {
        public const string AuditSuccessfullSaved = "DPSE-1000";
        public const string UnAuthorizedAccess = "DPSE-1001";
        public const string InputValidationError = "DPSE-1002";
        public const string InternalProcessingError = "DPSE-1003";
        public const string DatabaseOrFileAccessError = "DPSE-1004";
    }    
}