using System.Collections.Generic;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;

namespace FXA.DPSE.Service.PaymentValidation.Core
{
    public interface IValidationComposite
    {
        Dictionary<string, IValidationHandler> Services { get;}  
    }
}