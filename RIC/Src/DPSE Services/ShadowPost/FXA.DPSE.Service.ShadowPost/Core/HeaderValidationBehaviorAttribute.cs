using FXA.DPSE.Framework.Service.WCF.Attributes;

namespace FXA.DPSE.Service.ShadowPost.Core
{
    public class HeaderValidationBehaviorAttribute : ServiceBehaviorBase
    {
        public HeaderValidationBehaviorAttribute() : base(new MessageHeaderValidator())
        {
        }
    }
}