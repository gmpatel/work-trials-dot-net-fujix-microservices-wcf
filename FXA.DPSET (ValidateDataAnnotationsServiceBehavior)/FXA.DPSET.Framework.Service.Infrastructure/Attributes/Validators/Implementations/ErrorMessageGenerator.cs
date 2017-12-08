using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Core;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Implementations
{
    public class ErrorMessageGenerator : IErrorMessageGenerator
    {
        public string GenerateErrorMessage(string operationName, IList<ValidationResult> validationResults)
        {
            if (operationName == null)
            {
                throw new ArgumentNullException("operationName");
            }

            if (validationResults == null)
            {
                throw new ArgumentNullException("validationResults");
            }

            if (!validationResults.Any())
            {
                throw new ArgumentException("At least one ValidationResult is required");
            }

            var errorMessageBuilder = new StringBuilder();

            errorMessageBuilder.AppendFormat("Service operation '{0}' failed due to validation errors : ", operationName, Environment.NewLine);

            foreach (var validationResult in validationResults)
            {
                errorMessageBuilder.AppendFormat("{0} ", validationResult.ErrorMessage, Environment.NewLine);
            }

            return errorMessageBuilder.ToString().Trim();
        }
    }
}