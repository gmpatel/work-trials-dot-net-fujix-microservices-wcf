using System.Configuration;
using FXA.DPSE.Service.DipsPayload.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsPayload.Common.Configuration.Section;

namespace FXA.DPSE.Service.DipsPayload.Common.Configuration
{
    public class DipsPayloadServiceConfiguration : IDipsPayloadServiceConfiguration
    {
        private readonly DipsPayloadConfigurationSection _config;

        public DipsPayloadServiceConfiguration()
        {
            _config = (DipsPayloadConfigurationSection)ConfigurationManager.GetSection("serviceConfig");
        }

        public PayloadFileSystemLocationElement PayloadFileSystemLocation
        {
            get { return _config.PayloadFileSystemLocation; }
            set { _config.PayloadFileSystemLocation = value; }
        }

        public PayloadAccountNumberElement PayloadAccountNumber
        {
            get { return _config.PayloadAccountNumber; }
            set { _config.PayloadAccountNumber = value; }
        }

        public PayloadBsbNumberElement PayloadBsbNumber
        {
            get { return _config.PayloadBsbNumber; }
            set { _config.PayloadBsbNumber = value; }
        }

        public PayloadTransactionCodeElement PayloadTransactionCode
        {
            get { return _config.PayloadTransactionCode; }
            set { _config.PayloadTransactionCode = value; }
        }

        public PayloadProcessingDetailsElement PayloadProcessingDetails
        {
            get { return _config.PayloadProcessingDetails; }
            set { _config.PayloadProcessingDetails = value; }
        }


        public PayloadVoucherTypeElement PayloadVoucherType
        {
            get { return _config.PayloadVoucherType; }
            set { _config.PayloadVoucherType = value; }
        }
    }
}