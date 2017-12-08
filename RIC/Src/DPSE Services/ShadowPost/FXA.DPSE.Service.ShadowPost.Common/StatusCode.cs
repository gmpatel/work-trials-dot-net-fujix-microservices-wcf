namespace FXA.DPSE.Service.ShadowPost.Common
{
    public class StatusCode
    {
        public const string ShadowPostSuccessful = "DPSE-7000";
        
        /// <summary>
        ///HttpStatusCode = 401
        /// </summary>
        public const string UserCannotAccessResource = "DPSE-7001";

        public const string InputValidationError = "DPSE-7002";
        public const string InternalProcessingError = "DPSE-7003";
        public const string DatabaseOrFileErrror = "DPSE-7004";

        /// <summary>
        /// IB-374
        /// </summary>
        public const string FailedOnAccountHolderName = "DPSE-7005";

        /// <summary>
        /// IB-375
        /// </summary>
        public const string FailedOnAccountNotBelongToNin = "DPSE-7006";

        /// <summary>
        /// IB-373
        /// </summary>
        public const string FailedOnAccountTypes  = "DPSE-7007";

        /// <summary>
        /// IB-099, HttpStatusCode = 500
        /// </summary>
        public const string ApiUnavailableDueToRunTimeException = "DPSE-7008";

        /// <summary>
        /// IB-093, HttpStatusCode = 500
        /// </summary>
        public const string ApiUnavailableDueToGlobalSwitchError = "DPSE-7009";

        /// <summary>
        /// IB-097, HttpStatusCode = 500
        /// </summary>
        public const string ApiUnavailableDueToBacknedError = "DPSE-7010";

        /// <summary>
        /// IB-094, HttpStatusCode = 500
        /// </summary>
        public const string ApiErrorDueToDatabaseException = "DPSE-7011";

        /// <summary>
        /// IB-111, HttpStatusCode = 403
        /// </summary>
        public const string UnAuthorizedAccess = "DPSE-7012";

        public const string DuplicateRequestError = "DPSE-7001";
    }
}
