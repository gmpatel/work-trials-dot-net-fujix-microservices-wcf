using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Implementers;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidationBehavior : Attribute, IServiceBehavior
    {
       private readonly IParameterInspector _validatingParameterInspector;

        public ValidationBehavior(string dpseErrorCode = "DPSE-400")
        {
            var validators = new List<IObjectValidator>
            {
                new NullCheckObjectValidator(),
                new DataAnnotationsObjectValidator(),
                new ValidatableObjectValidator()
            };

            var errorMessageGenerator = new ErrorMessageGenerator();

            _validatingParameterInspector = new ValidatingParameterInspector(validators, errorMessageGenerator, dpseErrorCode);
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var operations =
                from dispatcher in serviceHostBase.ChannelDispatchers.Cast<ChannelDispatcher>()
                from endpoint in dispatcher.Endpoints
                from operation in endpoint.DispatchRuntime.Operations
                select operation;

            foreach (var operation in operations)
            {
                operation.ParameterInspectors.Add(_validatingParameterInspector);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}