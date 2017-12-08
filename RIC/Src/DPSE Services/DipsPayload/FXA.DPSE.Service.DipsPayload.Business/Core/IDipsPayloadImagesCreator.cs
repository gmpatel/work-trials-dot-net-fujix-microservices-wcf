using System.IO;
using FXA.DPSE.Service.DipsPayload.DataAccess;

namespace FXA.DPSE.Service.DipsPayload.Business.Core
{
    public interface IDipsPayloadImagesCreator
    {
        bool GeneratePayloadImages(PaymentInstruction paymentInstruction, DirectoryInfo directory);
    }
}