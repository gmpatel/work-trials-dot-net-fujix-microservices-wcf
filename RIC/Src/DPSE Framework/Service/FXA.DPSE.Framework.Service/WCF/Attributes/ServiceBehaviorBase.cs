using System;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace FXA.DPSE.Framework.Service.WCF.Attributes
{
    public class ServiceBehaviorBase : Attribute, IServiceBehavior
    {
        private readonly IDispatchMessageInspector _dispatchMessageInspector;

        public ServiceBehaviorBase(IDispatchMessageInspector dispatchMessageInspector)
        {
            _dispatchMessageInspector = dispatchMessageInspector;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
            System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (var serviceHostChannelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = serviceHostChannelDispatcher as ChannelDispatcher;

                if (channelDispatcher != null)
                {
                    foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        if (endpointDispatcher.DispatchRuntime.MessageInspectors.All(p => p.GetType() != _dispatchMessageInspector.GetType()))
                        {
                            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(_dispatchMessageInspector);
                        }
                    }
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, 
            System.ServiceModel.ServiceHostBase serviceHostBase, 
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(ServiceDescription serviceDescription, 
            System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }
    }
}
