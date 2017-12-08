using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsPayload.DataAccess
{
    public interface IPaymentInstructionDataAccess : IDisposable
    {
        IList<PaymentInstruction> GetPaymentInstructions();
        PaymentInstruction GetPaymentInstruction(long id);
        bool UpdatePaymentInstructionBatchDetails(long id, DirectoryInfo directory, DateTime bathCreateDateTime);
    }
}