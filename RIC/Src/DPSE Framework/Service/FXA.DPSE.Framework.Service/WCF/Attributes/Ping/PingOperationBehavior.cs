using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Ping 
{
    public class PingOperationBehavior : IOperationBehavior 
    {
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation) 
        {
            dispatchOperation.Invoker = new PingInvoker();
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }
    }
}