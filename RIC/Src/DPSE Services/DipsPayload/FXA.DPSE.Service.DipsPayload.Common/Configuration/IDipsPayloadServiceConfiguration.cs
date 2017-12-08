using FXA.DPSE.Service.DipsPayload.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsPayload.Common.Configuration
{
    public interface IDipsPayloadServiceConfiguration
    {
        PayloadFileSystemLocationElement PayloadFileSystemLocation { get; set; }
        PayloadAccountNumberElement PayloadAccountNumber { get; set; }
        PayloadBsbNumberElement PayloadBsbNumber { get; set; }
        PayloadTransactionCodeElement PayloadTransactionCode { get; set; }
        PayloadVoucherTypeElement PayloadVoucherType { get; set; }
        PayloadProcessingDetailsElement PayloadProcessingDetails { get; set; }
    }
}