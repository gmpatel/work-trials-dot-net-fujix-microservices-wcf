using FXA.DPSE.Service.DipsPayload.Business.Entity;

namespace FXA.DPSE.Service.DipsPayload.Business.Core
{
    public interface IPayloadBatchCreator
    {
        DipsPayloadBusinessResult GeneratePayload();
        DipsPayloadSingleBusinessResult GeneratePayload(long paymentInstructionId);
    }
}