using FXA.DPSE.Service.DTO.Core.Response;
using System.Runtime.Serialization;

namespace FXA.DPSE.Service.DTO.CodelineRules
{
    [DataContract]
    public class CodelineRulesResponse : DpseResponseBase
    {
        public CodelineRulesResponse()
        {
        }
        public CodelineRulesResponse(string trackingId)
        {
            TrackingId = trackingId;
        }

        public CodelineRulesResponse(string trackingId, string code, string message)
            
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