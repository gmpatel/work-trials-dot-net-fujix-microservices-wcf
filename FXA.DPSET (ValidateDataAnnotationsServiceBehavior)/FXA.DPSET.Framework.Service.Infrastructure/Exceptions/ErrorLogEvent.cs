using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Exceptions
{
    public class ErrorLogEvent
    {
        public long EventId { get; set; }
        public string ShortDescription { get; set; }
        public string OperationalGuidance { get; set; }
    }
}