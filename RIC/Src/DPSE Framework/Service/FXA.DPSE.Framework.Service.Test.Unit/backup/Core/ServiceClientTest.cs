using System.ServiceModel;
using System.ServiceModel.Description;

namespace FXA.DPSE.Framework.Service.Test.Unit.Core
{
    public class ServiceClientTest<TTestService>
    {
        public ServiceClientTest(string baseAddress)
        {
            var factory =
                new ChannelFactory<TTestService>(new WebHttpBinding(),
                    new EndpointAddress(baseAddress));

            factory.Endpoint.Behaviors.Add(new WebHttpBehavior());

            TTestService proxy = factory.CreateChannel();
            this.Proxy = proxy;
        }

        public TTestService Proxy { get; set; }
    }
}