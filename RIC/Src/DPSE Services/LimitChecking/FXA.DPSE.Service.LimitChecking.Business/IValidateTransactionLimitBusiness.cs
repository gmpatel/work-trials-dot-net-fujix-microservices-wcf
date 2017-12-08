using FXA.DPSE.Service.LimitChecking.Business.BusinessEntity;

namespace FXA.DPSE.Service.LimitChecking.Business
{
    public interface IValidateTransactionLimitBusiness
    {
        TransactionLimitValidationResult ValidatePayloadTransactionLimit(ChequePayload chequePayload);
    }
}