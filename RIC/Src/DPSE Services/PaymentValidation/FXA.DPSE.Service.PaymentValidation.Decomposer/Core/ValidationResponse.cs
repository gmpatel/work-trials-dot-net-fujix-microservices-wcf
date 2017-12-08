using FXA.DPSE.Service.DTO.PaymentValidation;

namespace FXA.DPSE.Service.PaymentValidation.Decomposer.Core
{
    public class ValidationResponse
    {
        public ValidationResponse()
        {
            PaymentValidationResponse = new PaymentValidationResponse();
        }
        public bool IsSucceed { get; set; }
        public PaymentValidationResponse PaymentValidationResponse { get; set; }

        //TODO: Change the following error types to a single enum !
        public bool AuditError { get; set; }
        public bool LoggingError { get; set; }
        public bool InternalError { get; set; }
    }
}