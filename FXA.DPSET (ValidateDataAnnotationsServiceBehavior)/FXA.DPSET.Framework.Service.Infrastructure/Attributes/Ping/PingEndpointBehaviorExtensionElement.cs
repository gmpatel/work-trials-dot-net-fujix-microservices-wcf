using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Ping 
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