using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Ping;

namespace FXA.DPSET.UnitTests.ServiceInfrastructure
{
    public class ServiceClient<TTestService>
    {
        public ServiceClient(string baseAddress)
        {
            var factory =
                new ChannelFactory<TTestService>(new WebHttpBinding(),
                    new EndpointAddress(baseAddress));

            factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
            factory.Endpoint.Behaviors.Add(new PingEndpointBehavior());

            var proxy = factory.CreateChannel();
            Proxy = proxy;
        }

        public TTestService Proxy { get; set; }
    }
}