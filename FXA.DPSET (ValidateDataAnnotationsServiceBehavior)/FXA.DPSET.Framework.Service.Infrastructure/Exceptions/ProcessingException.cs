using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Exceptions
{
    public class ProcessingException : Exception
    {
        public ErrorLogEvent Error { get; set; }

        public ProcessingException()
        {
        }

        public ProcessingException(string message)
            : base(message)
        {
        }

        public ProcessingException(Exception innerException)
            : base(null, innerException)
        {
        }

        public ProcessingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class ProcessingException<TLogEvent> : ProcessingException where TLogEvent : ErrorLogEvent
    {
        public ProcessingException(TLogEvent error)
        {
            this.Error = error;
        }

        public ProcessingException(TLogEvent error, string message)
            : base(message)
        {
            this.Error = error;
        }

        public ProcessingException(TLogEvent error, Exception innerException)
            : base(error.ShortDescription, innerException)
        {
            this.Error = error;
        }

        public ProcessingException(TLogEvent error, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Error = error;
        }
    }
}
