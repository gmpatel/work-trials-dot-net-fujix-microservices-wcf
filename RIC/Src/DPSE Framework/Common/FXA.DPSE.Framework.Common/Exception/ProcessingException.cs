using System.Runtime.Serialization;

namespace FXA.DPSE.Framework.Common.Exception
{
    public class ProcessingException : System.Exception
    {
        public ProcessingException()
        {
        }

        public ProcessingException(string message)
            : base(message)
        {
        }

        public ProcessingException(System.Exception innerException)
            : base(null, innerException)
        {
        }

        public ProcessingException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        protected ProcessingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class ProcessingException<TError> : ProcessingException where TError : struct
    {
        public TError Error { get; private set; }
        public string ErrorCode { get; private set; }

        public ProcessingException(TError error)
            : base(error.ToString())
        {
            Error = error;
        }

        public ProcessingException(TError error, string message, string errorCode)
            : base(message)
        {
            Error = error;
            ErrorCode = errorCode;
        }

        public ProcessingException(TError error, System.Exception innerException, string errorCode)
            : base(error.ToString(), innerException)
        {
            Error = error;
            ErrorCode = errorCode;
        }

        public ProcessingException(TError error, string message, System.Exception innerException, string errorCode)
            : base(message, innerException)
        {
            Error = error;
            ErrorCode = errorCode;
        }
    }
}