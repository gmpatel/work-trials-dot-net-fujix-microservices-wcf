using System;
using System.Collections.Generic;
using System.IO;

namespace FXA.DPSE.Service.DipsTransport.DataAccess
{
    public interface IDipsTransportDataAccess : IDisposable
    {
        IList<PaymentInstruction> GetBatchedPaymentInstructions();
        Transmission CreateTransportTransmissionRecords(IList<PaymentInstruction> paymentInstructions);
        IList<Transmission> GetTransportTransmissionsToBeProcessed();
        PaymentInstruction GetPaymentInstruction(long id);
        bool UpdateTransmissionBatchWithFileInfo(long transmissionId, long paymentInstructionId, FileInfo file);
        bool UpdateTransmissionBatch(TransmissionBatch transmissionBatch);
        bool UpdateTransmission(long transmissionId, FileInfo file, string fileSHA);
        bool UpdateTransmission(Transmission transmission);
        bool UpdatePaymentInstructionsTransmitted(Transmission transmission, DateTime? dateTime);
        IList<Transmission> GetTransportedTransmissions(DateTime? dateTime);
    }
}