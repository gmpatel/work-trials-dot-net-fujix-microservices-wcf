using System;
using System.ServiceModel.Configuration;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Ping 
{
    public class PingEndpointBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior() 
        {
            return new PingEndpointBehavior();
        }

        public override Type BehaviorType 
        {
            get
            {
                return typeof (PingEndpointBehavior);
            }
        }
    }
}