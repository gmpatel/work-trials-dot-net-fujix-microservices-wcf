using System;
using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;

namespace FXA.DPSE.Service.PaymentInstruction.Business
{
    public interface IPaymentInstructionBusiness
    {
        PaymentInstructionStoreResult StorePaymentInstruction(PaymentInstructionBusinessData data, TrackingIdentifierResult trackingIdentifierResult);
        TrackingIdentifierResult AssignTrackingIdentifiers(PaymentInstructionBusinessData paymentInstructionBusinessData, List<string> trackingNumbers);
        PaymentInstructionStatusUpdateBusinessResult UpdateStatus(PaymentInstructionStatusUpdateBusinessData data);
        BusinessResult UpdatePaymentInstructionWithShadowPost(long paymentInstructionId, DateTime? shadowPostProcessingDate);
    }
}