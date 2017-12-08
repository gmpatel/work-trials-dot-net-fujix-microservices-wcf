using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Service.ShadowPost.Business.Entities
{
    public class ShadowPostBusinessResult : BusinessResult
    {
        public ShadowPostBusinessResult()
        {
            ShadowPostedCheques = new List<ShadowPostedChequeInfo>();
        }
        public List<ShadowPostedChequeInfo> ShadowPostedCheques { get; set; }
    }
}