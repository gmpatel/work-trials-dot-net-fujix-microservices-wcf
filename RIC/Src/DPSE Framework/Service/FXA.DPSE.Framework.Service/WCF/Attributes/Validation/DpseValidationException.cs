using System;
using System.ComponentModel.DataAnnotations;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Validation
{
    public class DpseValidationException : ValidationException
    {
        public DpseValidationException(string message, string errorCode, object[] inputs)
            : base(message)
        {
            ServiceValidationErrorCode = errorCode;
            ServiceInputs = inputs;
        }

        public object[] ServiceInputs { get; private set; }
        public string ServiceValidationErrorCode { get; private set; }
    }
}