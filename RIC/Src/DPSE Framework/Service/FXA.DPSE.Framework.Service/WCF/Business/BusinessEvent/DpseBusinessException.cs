namespace FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent
{
    public class DpseBusinessException
    {
        public DpseBusinessException()
        {
        }

        public DpseBusinessException(DpseBusinessExceptionType type, string errorCode, string message, string details)
        {
            ExceptionType = type;
            ErrorCode = errorCode;
            Message = message;
            Details = details;
        }

        public DpseBusinessExceptionType ExceptionType { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}

