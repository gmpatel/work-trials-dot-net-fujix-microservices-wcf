using System.Runtime.Serialization;
using FXA.DPSE.Service.DTO.Core.Response;

namespace FXA.DPSE.Service.DTO.Logging
{
    [DataContract]
    public class LoggingResponse : DpseResponseBase
    {
        public LoggingResponse()
        {
        }

        public LoggingResponse(string code, string loggingMessage)
        {
            Code = code;
            Message = loggingMessage;
        }

        public LoggingResponse(string trackingId, string code, string loggingMessage)
        {
            TrackingId = trackingId;
            Code = code;
            Message = loggingMessage;
        }
    }
}
