using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Exceptions
{
    public static class ErrorContexts
    {
        public static readonly ErrorLogEvent RequestValidationError = new ErrorLogEvent
        {
            EventId = 144,
            ShortDescription = "Input validation failed",
            OperationalGuidance = ""
        };

        public static readonly ErrorLogEvent StringNullError = new ErrorLogEvent
        {
            EventId = 211,
            ShortDescription = "String is null",
            OperationalGuidance = ""
        };
    }
}