using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Error
{
    public class ErrorBehaviorBase : Attribute, IServiceBehavior//, IEndpointBehavior
    {
        private readonly IErrorHandler _errorHandler;

        public ErrorBehaviorBase(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
                ApplyDispatchBehavior(dispatcher);
        }

        //public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        //{
        //    ApplyDispatchBehavior(endpointDispatcher.ChannelDispatcher);
        //}

        private void ApplyDispatchBehavior(ChannelDispatcher dispatcher)
        {
            if (dispatcher.ErrorHandlers.All(e => e.GetType() != _errorHandler.GetType()))
            {
                dispatcher.ErrorHandlers.Add(_errorHandler);
            }
        }

        //public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        //{
        //}
        //public void Validate(ServiceEndpoint endpoint)
        //{
        //}
        //public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        //{
        //}
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }
    }
}