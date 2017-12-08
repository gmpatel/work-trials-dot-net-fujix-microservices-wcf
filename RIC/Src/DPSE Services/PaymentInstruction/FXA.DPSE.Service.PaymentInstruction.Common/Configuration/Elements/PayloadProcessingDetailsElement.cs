using System.Configuration;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements
{
    public class PayloadProcessingDetailsElement : ConfigurationElement
    {
        [ConfigurationProperty("batchClient", DefaultValue = null, IsRequired = true)]
        public string BatchClient
        {
            get { return (string)base["batchClient"]; }
            set { base["batchClient"] = value; }
        }

        [ConfigurationProperty("batchType", DefaultValue = null, IsRequired = true)]
        public string BatchType
        {
            get { return (string)base["batchType"]; }
            set { base["batchType"] = value; }
        }

        [ConfigurationProperty("batchAccountNumber", DefaultValue = null, IsRequired = true)]
        public string BatchAccountNumber
        {
            get { return (string)base["batchAccountNumber"]; }
            set { base["batchAccountNumber"] = value; }
        }

        [ConfigurationProperty("workType", DefaultValue = null, IsRequired = true)]
        public string WorkType
        {
            get { return (string)base["workType"]; }
            set { base["workType"] = value; }
        }

        [ConfigurationProperty("unitId", DefaultValue = null, IsRequired = true)]
        public string UnitId
        {
            get { return (string)base["unitId"]; }
            set { base["unitId"] = value; }
        }

        [ConfigurationProperty("state", DefaultValue = null, IsRequired = true)]
        public string State
        {
            get { return (string)base["state"]; }
            set { base["state"] = value; }
        }

        [ConfigurationProperty("collectingBank", DefaultValue = null, IsRequired = true)]
        public string CollectingBank
        {
            get { return (string)base["collectingBank"]; }
            set { base["collectingBank"] = value; }
        }

        [ConfigurationProperty("captureBsb", DefaultValue = null, IsRequired = true)]
        public string CaptureBsb
        {
            get { return (string)base["captureBsb"]; }
            set { base["captureBsb"] = value; }
        }

        [ConfigurationProperty("source", DefaultValue = null, IsRequired = true)]
        public string Source
        {
            get { return (string)base["source"]; }
            set { base["source"] = value; }
        }

        [ConfigurationProperty("documentReferenceNumberPreFix", DefaultValue = null, IsRequired = true)]
        public string DocumentReferenceNumberPreFix
        {
            get { return (string)base["documentReferenceNumberPreFix"]; }
            set { base["documentReferenceNumberPreFix"] = value; }
        }

        [ConfigurationProperty("headerVoucherFrontImagePath", DefaultValue = null, IsRequired = true)]
        public string HeaderVoucherFrontImagePath
        {
            get { return (string)base["headerVoucherFrontImagePath"]; }
            set { base["headerVoucherFrontImagePath"] = value; }
        }

        [ConfigurationProperty("headerVoucherRearImagePath", DefaultValue = null, IsRequired = true)]
        public string HeaderVoucherRearImagePath
        {
            get { return (string)base["headerVoucherRearImagePath"]; }
            set { base["headerVoucherRearImagePath"] = value; }
        }

        [ConfigurationProperty("creditVoucherFrontImagePath", DefaultValue = null, IsRequired = true)]
        public string CreditVoucherFrontImagePath
        {
            get { return (string)base["creditVoucherFrontImagePath"]; }
            set { base["creditVoucherFrontImagePath"] = value; }
        }

        [ConfigurationProperty("creditVoucherRearImagePath", DefaultValue = null, IsRequired = true)]
        public string CreditVoucherRearImagePath
        {
            get { return (string)base["creditVoucherRearImagePath"]; }
            set { base["creditVoucherRearImagePath"] = value; }
        }
        
    }
}