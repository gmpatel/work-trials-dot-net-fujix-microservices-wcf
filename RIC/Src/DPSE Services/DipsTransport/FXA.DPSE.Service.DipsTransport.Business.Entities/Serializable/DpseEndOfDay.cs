using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FXA.DPSE.Service.DipsTransport.Business.Entities.Serializable
{
    [SerializableAttribute]
    [XmlRoot(ElementName = "ns:DPSEEndOfDay")]
    public class DpseEndOfDay
    {
        [XmlElement(ElementName = "transmissionDate")]
        public string TransmissionDate { get; set; }

        [XmlElement(ElementName = "numberOfTransmissions")]
        public string NumberOfTransmissions { get; set; }

        [XmlElement(ElementName = "batchFile")]
        public List<BatchFile> BatchFiles { get; set; }
    }

    public class BatchFile
    {
        [XmlElement(ElementName = "batchFileName")]
        public string BatchFileName { get; set; }

        [XmlElement(ElementName = "sha2Hash")]
        public string Sha2Hash { get; set; }

        [XmlElement(ElementName = "transmissionDateTime")]
        public string TransmissionDateTime { get; set; }

        [XmlElement(ElementName = "transactionCount")]
        public string TransactionCount { get; set; }

        [XmlElement(ElementName = "retryCount")]
        public string RetryCount { get; set; }
    }
}
