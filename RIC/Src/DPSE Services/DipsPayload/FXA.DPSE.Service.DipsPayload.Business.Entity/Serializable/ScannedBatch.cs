using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FXA.DPSE.Service.DipsPayload.Business.Entity.Serializable
{
    [SerializableAttribute]
    [XmlRoot(ElementName = "ns:ScannedBatch")]
    public class ScannedBatch
    {
        [XmlElement(ElementName = "client")]
        public string Client { get; set; }

        [XmlElement(ElementName = "processingDate")]
        public string ProcessingDate { get; set; }

        [XmlElement(ElementName = "batchNumber")]
        public string BatchNumber { get; set; }

        [XmlElement(ElementName = "batchType")]
        public string BatchType { get; set; }

        [XmlElement(ElementName = "subBatchType")]
        public string SubBatchType { get; set; }

        [XmlElement(ElementName = "operator")]
        public string Operator { get; set; }

        [XmlElement(ElementName = "unitID")]
        public string UnitId { get; set; }

        [XmlElement(ElementName = "processingState")]
        public string ProcessingState { get; set; }

        [XmlElement(ElementName = "collectingBank")]
        public string CollectingBank { get; set; }

        [XmlElement(ElementName = "workType")]
        public string WorkType { get; set; }

        [XmlElement(ElementName = "captureBsb")]
        public string CaptureBsb { get; set; }

        [XmlElement(ElementName = "voucher")] // [XmlArray("vouchers"), XmlArrayItem(typeof(BatchVoucher), ElementName = "voucher")]
        public List<BatchVoucher> BatchVouchers { get; set; }

        [XmlElement(ElementName = "source")]
        public string Source { get; set; }
    }

    [SerializableAttribute]
    public class BatchVoucher
    {
        [XmlElement(ElementName = "rawOCR")]
        public string RawOCR { get; set; }

        [XmlElement(ElementName = "rawMICR")]
        public string RawMICR { get; set; }

        [XmlElement(ElementName = "micrFlag")]
        public string MicrFlag { get; set; }

        [XmlElement(ElementName = "micrUnprocessableFlag")]
        public string MicrUnprocessableFlag { get; set; }

        [XmlElement(ElementName = "micrSuspectFraudFlag ")]
        public string MicrSuspectFraudFlag { get; set; }

        [XmlElement(ElementName = "traceID")]
        public string TraceId { get; set; }

        [XmlElement(ElementName = "processingDate")]
        public string ProcessingDate { get; set; }

        [XmlElement(ElementName = "documentType")]
        public string DocumentType { get; set; }

        [XmlElement(ElementName = "transactionCode")]
        public string TransactionCode { get; set; }

        [XmlElement(ElementName = "documentReferenceNumber")]
        public string DocumentReferenceNumber { get; set; }

        [XmlElement(ElementName = "bsbNumber")]
        public string BsbNumber { get; set; }

        [XmlElement(ElementName = "auxDom")]
        public string AuxDom { get; set; }

        [XmlElement(ElementName = "extraAuxDom")]
        public string ExtraAuxDom { get; set; }

        [XmlElement(ElementName = "amount")]
        public string Amount { get; set; }

        [XmlElement(ElementName = "accountNumber")]
        public string AccountNumber { get; set; }

        [XmlElement(ElementName = "inactiveFlag")]
        public string InactiveFlag { get; set; }
    }
}