using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Service.Logging.Business.BusinessEntity
{
    public class LoggingBusinessResult : BusinessResult
    {
        public LoggingBusinessResult()
        {
        }

        public LoggingBusinessResult(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string TrackingId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
