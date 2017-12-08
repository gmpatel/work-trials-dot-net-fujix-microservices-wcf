using System.IO;
using FXA.DPSE.Service.DipsPayload.DataAccess;

namespace FXA.DPSE.Service.DipsPayload.Business.Core
{
    public interface IDipsPayloadMetadataCreator
    {
        void GetScannedBatchMetadata(PaymentInstruction paymentInstruction, DirectoryInfo payloadDirectory);
    }
}