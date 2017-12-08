using System.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Section
{
    public class PaymentInstructionConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("traceTrackingService")]
        public TraceTrackingServiceElement TraceTrackingService
        {
            get { return ((TraceTrackingServiceElement)(base["traceTrackingService"])); }
            set { base["traceTrackingService"] = value; }
        }

        [ConfigurationProperty("shadowPostService")]
        public ShadowPostServiceElement ShadowPostService
        {
            get { return ((ShadowPostServiceElement)(base["shadowPostService"])); }
            set { base["shadowPostService"] = value; }
        }

        [ConfigurationProperty("dipsPayloadService")]
        public DipsPayloadServiceElement DipsPayloadService
        {
            get { return ((DipsPayloadServiceElement)(base["dipsPayloadService"])); }
            set { base["dipsPayloadService"] = value; }
        }

        [ConfigurationProperty("paymentValidationService")]
        public PaymentValidationServiceElement PaymentValidationService
        {
            get { return ((PaymentValidationServiceElement)(base["paymentValidationService"])); }
            set { base["paymentValidationService"] = value; }
        }

        [ConfigurationProperty("payloadAccountNumber")]
        public PayloadAccountNumberElement PayloadAccountNumber
        {
            get { return ((PayloadAccountNumberElement)(base["payloadAccountNumber"])); }
            set { base["payloadAccountNumber"] = value; }
        }

        [ConfigurationProperty("payloadBsbNumber")]
        public PayloadBsbNumberElement PayloadBsbNumber
        {
            get { return ((PayloadBsbNumberElement)(base["payloadBsbNumber"])); }
            set { base["payloadBsbNumber"] = value; }
        }

        [ConfigurationProperty("payloadTransactionCode")]
        public PayloadTransactionCodeElement PayloadTransactionCode
        {
            get { return ((PayloadTransactionCodeElement)(base["payloadTransactionCode"])); }
            set { base["payloadTransactionCode"] = value; }
        }

        [ConfigurationProperty("payloadVoucherType")]
        public PayloadVoucherTypeElement PayloadVoucherType
        {
            get { return ((PayloadVoucherTypeElement)(base["payloadVoucherType"])); }
            set { base["payloadVoucherType"] = value; }
        }

        [ConfigurationProperty("payloadProcessingDetails")]
        public PayloadProcessingDetailsElement PayloadProcessingDetails
        {
            get { return ((PayloadProcessingDetailsElement)(base["payloadProcessingDetails"])); }
            set { base["payloadProcessingDetails"] = value; }
        }

        [ConfigurationProperty("headerValidation")]
        public HeaderValidationElement HeaderValidation
        {
            get { return ((HeaderValidationElement)(base["headerValidation"])); }
            set { base["headerValidation"] = value; }
        }

        [ConfigurationProperty("dipsTransportProcessingTimeRange")]
        public DipsTransportProcessingTimeRangeElement DipsTransportProcessingTimeRange
        {
            get { return ((DipsTransportProcessingTimeRangeElement)(base["dipsTransportProcessingTimeRange"])); }
            set { base["dipsTransportProcessingTimeRange"] = value; }
        }
    }
}