using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;

namespace FXA.DPSE.Framework.Service.WCF.Host
{
    public class RestServiceHostFactory<TServiceContract> : AutofacWebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = new ServiceHost(serviceType, baseAddresses);
            var endpoint = host.AddServiceEndpoint(typeof(TServiceContract), new WebHttpBinding(), "");

            endpoint.Behaviors.Add(new WebHttpBehavior());

            return host;
        }
    }
}