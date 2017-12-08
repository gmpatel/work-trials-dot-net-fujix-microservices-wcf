using System.Collections.Generic;
using FXA.DPSE.Service.DTO.PaymentInstruction;

namespace FXA.DPSE.Service.PaymentInstruction.Facade.Core
{
    public class PaymentInstructionRequestWrapper
    {
        public PaymentInstructionRequest Content { get; set; }
        public IDictionary<string, string> Header { get; set; }
    }
}