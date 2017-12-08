using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Core;
using FXA.DPSET.Framework.Service.Infrastructure.Exceptions;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Implementations
{
    public class ValidatingParameterInspector : IParameterInspector
    {
        private readonly IEnumerable<IObjectValidator> _validators;
        private readonly IErrorMessageGenerator _errorMessageGenerator;

        public ValidatingParameterInspector(IList<IObjectValidator> validators, IErrorMessageGenerator errorMessageGenerator)
        {
            if (validators == null)
            {
                throw new ArgumentNullException("validators");
            }

            if (!validators.Any())
            {
                throw new ArgumentException("At least one validator is required.");
            }

            if (errorMessageGenerator == null)
            {
                throw new ArgumentNullException("errorMessageGenerator");
            }

            _validators = validators;
            _errorMessageGenerator = errorMessageGenerator;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            var validationResults = new List<ValidationResult>();

            foreach (var input in inputs)
            {
                foreach (var validator in _validators)
                {
                    var results = validator.Validate(input);
                    validationResults.AddRange(results);
                }
            }

            if (validationResults.Count > 0)
            {
                var message = _errorMessageGenerator.GenerateErrorMessage(operationName, validationResults);
                throw new ProcessingException<ErrorLogEvent>(ErrorContexts.RequestValidationError, message);
            }

            return null;
        }
    }
}