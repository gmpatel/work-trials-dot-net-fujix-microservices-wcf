using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Service.ShadowPost.Business.Entities
{
    public class DuplicateIdentificationBusinessResult : BusinessResult
    {
        public bool IsDuplicated { get; set; }
    }
}