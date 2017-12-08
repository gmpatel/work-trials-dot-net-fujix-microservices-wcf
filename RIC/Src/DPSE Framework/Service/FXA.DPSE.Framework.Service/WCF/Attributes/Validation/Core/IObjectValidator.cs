using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core
{
    public interface IObjectValidator
    {
        IEnumerable<ValidationResult> Validate(object value);
    }
}