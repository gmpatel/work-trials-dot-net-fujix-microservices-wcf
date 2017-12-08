using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Implementers
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

            errorMessageBuilder.Append("Message received was improperly formatted. Invalid request. ");

            foreach (var validationResult in validationResults)
            {
                var errorMessage = validationResult.ErrorMessage.Trim();
                errorMessageBuilder.AppendFormat("{0}", errorMessage);
            }

            var message = errorMessageBuilder.ToString().Trim();
            if (message.EndsWith(", ")) message = message.Substring(0, message.Length - 2);

            return message;
        }
    }
}