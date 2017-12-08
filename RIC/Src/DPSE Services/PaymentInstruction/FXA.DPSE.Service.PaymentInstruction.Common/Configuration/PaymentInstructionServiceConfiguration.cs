using FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements;
using System.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Section;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration
{
    public class PaymentInstructionServiceConfiguration : IPaymentInstructionServiceConfiguration
    {
        private readonly PaymentInstructionConfigurationSection _config;

        public PaymentInstructionServiceConfiguration()
        {
            _config = (PaymentInstructionConfigurationSection)ConfigurationManager.GetSection("serviceConfig");
        }

        public TraceTrackingServiceElement TraceTrackingService{
            get { return _config.TraceTrackingService; }
        }

        public PayloadAccountNumberElement PayloadAccountNumber
        {
            get { return _config.PayloadAccountNumber; }
        }

        public PayloadBsbNumberElement PayloadBsbNumber
        {
            get { return _config.PayloadBsbNumber; }
        }

        public PayloadTransactionCodeElement PayloadTransactionCode
        {
            get { return _config.PayloadTransactionCode; }
        }

        public PayloadProcessingDetailsElement PayloadProcessingDetails
        {
            get { return _config.PayloadProcessingDetails; }
        }

        public PayloadVoucherTypeElement PayloadVoucherType
        {
            get { return _config.PayloadVoucherType; }
        }

        public PaymentValidationServiceElement PaymentValidationService
        {
            get { return _config.PaymentValidationService; }
        }

        public ShadowPostServiceElement ShadowPostService
        {
            get { return _config.ShadowPostService; }
        }

        public DipsPayloadServiceElement DipsPayloadService
        {
            get { return _config.DipsPayloadService; }
        }

        public HeaderValidationElement HeaderValidation
        {
            get { return _config.HeaderValidation; }
        }

        public DipsTransportProcessingTimeRangeElement DipsTransportProcessingTimeRange
        {
            get { return _config.DipsTransportProcessingTimeRange; }
        }
    }
}