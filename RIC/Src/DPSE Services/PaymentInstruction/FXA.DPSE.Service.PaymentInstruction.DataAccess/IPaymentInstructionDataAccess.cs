using System;
using System.Collections.Generic;

namespace FXA.DPSE.Service.PaymentInstruction.DataAccess
{
    public interface IPaymentInstructionDataAccess : IDisposable
    {
        Account GetOrCreateAccount(Account account, IList<string> accountNames, string trackingId, string channelType, string sessionId);
        IDictionary<Voucher, VoucherImage> SaveVouchers(IDictionary<Voucher, VoucherImage> vouchers, string trackingId, string channelType, string sessionId);
        ClientSession CreateSession(ClientSession session, string trackingId, string channelType);
        PaymentInstruction CreatePaymentInstruction(PaymentInstruction paymentInstruction);
        PaymentInstruction UpdatePaymentInstructionStatus(PaymentInstruction paymentInstruction);

        void UpdatePaymentInstructionProcessingDate(long id, DateTime processingDate);
    }
}