using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Response;

namespace FXA.DPSE.Service.DTO.DuplicateIdentification
{
    [DataContract]
    public class DuplicateIdentificationResponse : DpseResponseBase
    {
        public DuplicateIdentificationResponse()
        {
            
        }
        public DuplicateIdentificationResponse(string trackingId)
        {
            TrackingId = trackingId;
        }

        public DuplicateIdentificationResponse(string trackingId, string code, string message)
            
        {
            TrackingId = trackingId;
            Code = code;
            Message = message;
        }

        [DataMember(Name = "cheque_responses", Order = 4)]
        public ChequeResponse[] Cheques { get; set; }
    }

    [DataContract]
    public class ChequeResponse
    {
        public ChequeResponse()
        {
            
        }
        public ChequeResponse(int sequenceId, string chequeResponseCode, string chequeResponseDescription)
        {
            SequenceId = sequenceId;
            ChequeResponseCode = chequeResponseCode;
            ChequeResponseDescription = chequeResponseDescription;
        }

        [DataMember(Name = "sequence_id", Order = 1)]
        public int SequenceId { get; set; }

        [DataMember(Name = "cheque_response_code", Order = 2)]
        public string ChequeResponseCode { get; set; }

        [DataMember(Name = "cheque_response_description", Order = 3)]
        public string ChequeResponseDescription { get; set; }
    }
}