using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsPayload.Business.Entity
{
    public class DipsPayloadBusinessData
    {
        public string MessageVersion { get; set; }
        public string RequestDateTimeUtc { get; set; }
        public string IpAddressV4 { get; set; }
        public string ClientName { get; set; }
    }
}