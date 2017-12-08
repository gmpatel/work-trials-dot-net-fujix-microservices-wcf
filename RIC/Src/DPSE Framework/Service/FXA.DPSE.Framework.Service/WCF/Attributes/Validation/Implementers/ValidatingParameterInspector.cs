using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.Dispatcher;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Implementers
{
    public class ValidatingParameterInspector : IParameterInspector
    {
        private readonly IEnumerable<IObjectValidator> _validators;
        private readonly IErrorMessageGenerator _errorMessageGenerator;
        private readonly string _dpseErrorCode;

        public ValidatingParameterInspector(IList<IObjectValidator> validators, IErrorMessageGenerator errorMessageGenerator, string dpseErrorCode)
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
            _dpseErrorCode = dpseErrorCode;
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
                var message = string.Format( _errorMessageGenerator.GenerateErrorMessage(operationName, validationResults));

                throw new DpseValidationException(message, _dpseErrorCode, inputs);
            }

            return null;
        }
    }
}