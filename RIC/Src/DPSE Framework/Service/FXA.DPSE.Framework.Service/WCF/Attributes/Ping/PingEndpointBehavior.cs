using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Ping 
{
    public class PingEndpointBehavior : Attribute, IEndpointBehavior
    {
        #region IEndpointConstants

        private const string PingOperationName = "Ping";
        private const string PingResponse = "PingResponse";

        #endregion

        #region IEndpointBehavior

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) 
        {
            if (PingOperationNotDeclaredInContract(endpoint.Contract))
            {
                AddPingToContractDescription(endpoint.Contract);
            }

            UpdateContractFilter(endpointDispatcher, endpoint.Contract);
            AddPingToDispatcher(endpointDispatcher, endpoint.Contract);
        }

        private static bool PingOperationNotDeclaredInContract(ContractDescription contract) 
        {
            return ! (contract.Operations.Any(
                operationDescription => operationDescription.Name.Equals(PingOperationName, StringComparison.InvariantCultureIgnoreCase))
            );
        }

        private static void AddPingToContractDescription(ContractDescription contractDescription) 
        {
            var pingOperationDescription = new OperationDescription(PingOperationName, contractDescription);
            var inputMessageDescription = new MessageDescription(GetAction(contractDescription, PingOperationName), MessageDirection.Input);
            var outputMessageDescription = new MessageDescription(GetAction(contractDescription, PingResponse), MessageDirection.Output);

            var returnValue = new MessagePartDescription("PingResult", contractDescription.Namespace)
            {
                Type = typeof (string)
            };

            inputMessageDescription.Body.WrapperName = PingOperationName;
            inputMessageDescription.Body.WrapperNamespace = contractDescription.Namespace;
            
            outputMessageDescription.Body.ReturnValue = returnValue;
            outputMessageDescription.Body.WrapperName = PingResponse;
            outputMessageDescription.Body.WrapperNamespace = contractDescription.Namespace;

            pingOperationDescription.Messages.Add(inputMessageDescription);
            pingOperationDescription.Messages.Add(outputMessageDescription);

            pingOperationDescription.Behaviors.Add(new DataContractSerializerOperationBehavior(pingOperationDescription));
            pingOperationDescription.Behaviors.Add(new PingOperationBehavior());
            pingOperationDescription.Behaviors.Add(new WebInvokeAttribute
            {
                Method = "GET",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json,
                UriTemplate = "ping",
            });

            contractDescription.Operations.Add(pingOperationDescription);
        }

        private static void UpdateContractFilter(EndpointDispatcher endpointDispatcher, ContractDescription contractDescription) 
        {
            var actions = (from operationDescription in contractDescription.Operations select GetAction(contractDescription, operationDescription.Name)).ToArray();
            endpointDispatcher.ContractFilter = new ActionMessageFilter(actions);
        }
        
        private static void AddPingToDispatcher(EndpointDispatcher endpointDispatcher, ContractDescription contractDescription) {
            var pingDispatchOperation = new DispatchOperation(endpointDispatcher.DispatchRuntime,
                PingOperationName,
                GetAction(contractDescription, PingOperationName),
                GetAction(contractDescription, PingResponse))
            {
                Invoker = new PingInvoker()
            };

            endpointDispatcher.DispatchRuntime.Operations.Add(pingDispatchOperation);
        }        

        private static string GetAction(ContractDescription contractDescription, string name) 
        {
            var nameSpace = contractDescription.Namespace;

            if (!nameSpace.EndsWith("/"))
            {
                nameSpace = nameSpace + "/";
            }

            var action = string.Format("{0}{1}/{2}", nameSpace, contractDescription.Name, name);
            
            Trace.WriteLine(string.Format("Action '{0}'", action));
            
            return action;
        }

        #endregion
    }
}