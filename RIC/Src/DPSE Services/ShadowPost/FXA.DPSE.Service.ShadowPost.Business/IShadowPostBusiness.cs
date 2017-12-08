using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Service.ShadowPost.Business.Entities;

namespace FXA.DPSE.Service.ShadowPost.Business
{
    public interface IShadowPostBusiness
    {
        DuplicateIdentificationBusinessResult CheckRequestDuplication(PayloadInfo data);
        BusinessResult StoreShadowPostRequest(PayloadInfo payloadInfo);
        ShadowPostBusinessResult ProcessChequeProcessingDate(PayloadInfo payloadInfo, List<ShadowPostedChequeInfo> shadowPostedCheques);
    }
}
