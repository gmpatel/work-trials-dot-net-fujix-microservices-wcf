using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Core;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Implementations;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateDataAnnotationsServiceBehavior : Attribute, IServiceBehavior
    {
        private readonly IParameterInspector _validatingParameterInspector;

        public ValidateDataAnnotationsServiceBehavior()
        {
            var validators = new List<IObjectValidator>
            {
                new NullCheckObjectValidator(),
                new DataAnnotationsObjectValidator(),
                new ValidatableObjectValidator()
            };

            var errorMessageGenerator = new ErrorMessageGenerator();

            _validatingParameterInspector = new ValidatingParameterInspector(validators, errorMessageGenerator);
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