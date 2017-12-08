using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.PaymentInstruction.Business.Entities
{
    public class PaymentInstructionStatusUpdateBusinessData
    {
        public string MessageVersion { get; set; }
        public string RequestDateTimeUtc { get; set; }
        public string Id { get; set; }
        public string TrackingId { get; set; }
        public string ChannelType { get; set; }
        public string Status { get; set; }
        
        public PaymentInstructionSession ClientSession { get; set; }

        public IDictionary<string, string> Headers { get; set; } 
    }
}
