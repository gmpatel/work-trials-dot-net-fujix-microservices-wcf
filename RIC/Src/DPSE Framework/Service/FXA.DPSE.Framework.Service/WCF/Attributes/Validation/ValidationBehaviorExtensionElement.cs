using System;
using System.ServiceModel.Configuration;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation
{
    public class ValidationBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(ValidationBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new ValidationBehavior();
        }
    }
}   