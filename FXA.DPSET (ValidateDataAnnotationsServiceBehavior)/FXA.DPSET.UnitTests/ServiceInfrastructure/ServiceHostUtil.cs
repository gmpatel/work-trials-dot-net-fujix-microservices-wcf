using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.UnitTests.ServiceInfrastructure
{
    public static class ServiceHostUtil
    {
        public static ServiceHost CreateServiceHost<TServiceToHost>(
            TServiceToHost serviceToHost,
            Uri baseAddress,
            string endpointAddress)
            where TServiceToHost : class
        {
            ServiceHost serviceHost =
                new ServiceHost(serviceToHost, new[]
                    {
                        baseAddress
                    });

            serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
            serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>().InstanceContextMode = InstanceContextMode.Single;

            ServiceEndpoint endPoint =
                serviceHost.AddServiceEndpoint(typeof(TServiceToHost), new WebHttpBinding(), endpointAddress);
            endPoint.Behaviors.Add(new WebHttpBehavior());

            serviceHost.Open();
            return serviceHost;
        }
    }
}