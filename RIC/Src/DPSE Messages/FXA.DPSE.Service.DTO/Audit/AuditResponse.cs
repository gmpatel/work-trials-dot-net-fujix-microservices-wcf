using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Response;
using Newtonsoft.Json;

namespace FXA.DPSE.Service.DTO.Audit
{
    [DataContract]
    public class AuditResponse : DpseResponseBase
    {
        public AuditResponse()
        {
            
        }

        public AuditResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }
        
        public AuditResponse(string trackingId)
        {
            TrackingId = trackingId;
        }

        public AuditResponse(string trackingId, string externalCorrelationId, string documentReferenceNumber)
        {
            TrackingId = trackingId;
            ExternalCorrelationId = externalCorrelationId;
            DocumentReferenceNumber = documentReferenceNumber;
        }
    }
}