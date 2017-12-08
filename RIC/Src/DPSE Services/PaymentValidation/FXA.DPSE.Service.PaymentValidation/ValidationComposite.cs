using System.Collections.Generic;
using FXA.DPSE.Service.PaymentValidation.Core;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;

namespace FXA.DPSE.Service.PaymentValidation
{
    public class ValidationComposite : IValidationComposite
    {
        public Dictionary<string, IValidationHandler> Services { get; set; }  
    }
}