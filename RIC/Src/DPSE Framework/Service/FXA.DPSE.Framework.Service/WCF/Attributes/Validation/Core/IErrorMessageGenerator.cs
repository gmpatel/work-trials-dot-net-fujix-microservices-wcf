using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core
{
    public interface IErrorMessageGenerator
    {
        string GenerateErrorMessage(string operationName, IList<ValidationResult> validationResults);
    }
}