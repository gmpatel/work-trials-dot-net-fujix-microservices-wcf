using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Service.DipsTransport.Business.Entities;

namespace FXA.DPSE.Service.DipsTransport.Business.Entities
{
    public class DipsTransportBusinessResult : BusinessResult
    {
        private readonly IList<DipsTransportBusinessInfo> _businessInfoList = new List<DipsTransportBusinessInfo>();

        public bool HasInfo
        {
            get { return _businessInfoList.Count > 0; }
        }

        public void AddBusinessInfos(IEnumerable<DipsTransportBusinessInfo> infos)
        {
            infos.ToList().ForEach(info => _businessInfoList.Add(info));
        }

        public void AddBusinessInfo(DipsTransportBusinessInfo info)
        {
            _businessInfoList.Add(info);
        }

        public IList<DipsTransportBusinessInfo> BusinessInfos()
        {
            return _businessInfoList;
        }

        public string MessageVersion { get; set; }
        public string RequestUtc { get; set; }
        public string RequestGuid { get; set; }
        public string IpAddressV4 { get; set; }
        public string ReportDate { get; set; }
    }
}