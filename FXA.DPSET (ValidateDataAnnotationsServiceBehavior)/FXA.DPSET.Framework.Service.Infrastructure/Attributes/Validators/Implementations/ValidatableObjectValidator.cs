using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Core;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Implementations
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