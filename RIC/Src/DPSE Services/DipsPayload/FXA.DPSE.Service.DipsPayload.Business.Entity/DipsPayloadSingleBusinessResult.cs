using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Service.DipsPayload.Business.Entity
{
    public class DipsPayloadSingleBusinessResult : BusinessResult
    {
        public long PaymentInstructionId { get; set; }
        public string ResultStatus { get; set; }
    }
}