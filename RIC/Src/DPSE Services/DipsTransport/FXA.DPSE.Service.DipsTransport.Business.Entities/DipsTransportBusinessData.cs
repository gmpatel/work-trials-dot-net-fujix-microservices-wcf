using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsTransport.Business.Entities
{
    public class DipsTransportBusinessData
    {
        public string MessageVersion { get; set; }
        public string RequestUtc { get; set; }
        public string RequestGuid { get; set; }
        public string IpAddressV4 { get; set; }
        public string ReportDate { get; set; }
    }
}