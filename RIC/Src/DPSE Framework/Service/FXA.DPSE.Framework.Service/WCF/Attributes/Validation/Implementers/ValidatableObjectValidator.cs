using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Implementers
{
    public class ValidatableObjectValidator : IObjectValidator
    {
        public IEnumerable<ValidationResult> Validate(object value)
        {
            var validatableInput = value as IValidatableObject;

            if (validatableInput == null) 
                yield break;

            var context = new ValidationContext(value, null, null);

            foreach (var result in validatableInput.Validate(context))
            {
                yield return result;
            }
        }
    }
}