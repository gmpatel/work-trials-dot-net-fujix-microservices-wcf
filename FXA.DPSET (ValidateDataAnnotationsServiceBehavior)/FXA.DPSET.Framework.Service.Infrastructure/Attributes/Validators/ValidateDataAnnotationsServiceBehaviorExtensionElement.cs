using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators
{
    public class ValidateDataAnnotationsServiceBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(ValidateDataAnnotationsServiceBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new ValidateDataAnnotationsServiceBehavior();
        }
    }
}   