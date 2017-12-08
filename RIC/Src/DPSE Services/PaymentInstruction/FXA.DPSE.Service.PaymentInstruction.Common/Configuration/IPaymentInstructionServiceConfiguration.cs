using FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration
{
    public interface IPaymentInstructionServiceConfiguration
    {
        TraceTrackingServiceElement TraceTrackingService { get; }
        PaymentValidationServiceElement PaymentValidationService { get; }
        ShadowPostServiceElement ShadowPostService { get; }
        DipsPayloadServiceElement DipsPayloadService { get; }
        PayloadAccountNumberElement PayloadAccountNumber { get; }
        PayloadBsbNumberElement PayloadBsbNumber { get; }
        PayloadTransactionCodeElement PayloadTransactionCode { get; }
        PayloadVoucherTypeElement PayloadVoucherType { get; }
        PayloadProcessingDetailsElement PayloadProcessingDetails { get; }
        DipsTransportProcessingTimeRangeElement DipsTransportProcessingTimeRange { get; }
    }
}