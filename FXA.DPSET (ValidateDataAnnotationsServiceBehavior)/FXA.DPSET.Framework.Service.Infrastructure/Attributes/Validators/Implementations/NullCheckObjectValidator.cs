using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Core;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Implementations
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