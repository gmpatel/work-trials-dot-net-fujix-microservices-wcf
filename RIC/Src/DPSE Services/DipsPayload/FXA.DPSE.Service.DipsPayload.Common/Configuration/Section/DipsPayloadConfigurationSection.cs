using System.Configuration;
using FXA.DPSE.Service.DipsPayload.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsPayload.Common.Configuration.Section
{
    public class DipsPayloadConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("payloadFileSystemLocation")]
        public PayloadFileSystemLocationElement PayloadFileSystemLocation
        {
            get { return ((PayloadFileSystemLocationElement)(base["payloadFileSystemLocation"])); }
            set { base["payloadFileSystemLocation"] = value; }
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
    }
}