using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators.Core
{
    public interface IErrorMessageGenerator
    {
        string GenerateErrorMessage(string operationName, IList<ValidationResult> validationResults);
    }
}