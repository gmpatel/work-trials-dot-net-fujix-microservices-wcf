using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace FXA.DPSE.Framework.Service.WCF
{
    public static class OperationContextExtensions
    {
        public static string GetMethodName(this OperationContext operationContext)
        {
            var methodName = string.Empty;
            var bindingName = operationContext.EndpointDispatcher.ChannelDispatcher.BindingName;
            if (bindingName.Contains("WebHttpBinding"))
            {
                //REST request
                methodName = (string)operationContext.IncomingMessageProperties["HttpOperationName"];
            }
            else
            {
                //SOAP request
                var action = operationContext.IncomingMessageHeaders.Action;
                var dispatchOperation = operationContext.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o => o.Action == action);
                if (dispatchOperation != null)
                    methodName = dispatchOperation.Name;
            }
            return methodName;
        }

        public static OperationDescription GetOperationDescription(this OperationContext operationContext)
        {
            var methodName = GetMethodName(operationContext);
            if (string.IsNullOrEmpty(methodName)) return null;

            OperationDescription operationDescription = null;

            var endpointAddress = operationContext.EndpointDispatcher.EndpointAddress;
            var hostDescription = operationContext.Host.Description;
            var serviceEndpoint = hostDescription.Endpoints.Find(endpointAddress.Uri);

            if (serviceEndpoint != null)
            {
                operationDescription = serviceEndpoint.Contract.Operations.Find(methodName);
            }

            return operationDescription;
        }

        public static string GetClientAddress(this OperationContext operationContext)
        {
            var address = string.Empty;
            var endpointProperty = operationContext.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (endpointProperty != null)
            {
                address = string.Concat(endpointProperty.Address, ":", endpointProperty.Port);
            }

            return address;
        }
    }
}
