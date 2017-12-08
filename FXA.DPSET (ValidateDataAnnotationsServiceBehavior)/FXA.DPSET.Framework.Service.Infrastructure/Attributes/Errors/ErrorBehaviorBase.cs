using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Errors
{
    public class ErrorBehaviorBase : Attribute, IServiceBehavior, IEndpointBehavior
    {
        private readonly IErrorHandler _errorHandler;

        public ErrorBehaviorBase(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
                this.ApplyDispatchBehavior(dispatcher);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            this.ApplyDispatchBehavior(endpointDispatcher.ChannelDispatcher);
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            this.ApplyClientBehavior(clientRuntime);
        }


        private void ApplyDispatchBehavior(ChannelDispatcher dispatcher)
        {
            // Don’t add an error handler if it already exists
            if (dispatcher.ErrorHandlers.All(e => e.GetType() != _errorHandler.GetType()))
            {
                dispatcher.ErrorHandlers.Add(_errorHandler);
            }
        }

        private void ApplyClientBehavior(ClientRuntime runtime)
        {
            //we are not a
        }
    }
}