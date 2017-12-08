using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Implementers
{
    public class NullCheckObjectValidator : IObjectValidator
    {
        public IEnumerable<ValidationResult> Validate(object value)
        {
            if (value == null)
            {
                yield return new ValidationResult("Input is null.");
            }
        }
    }
}