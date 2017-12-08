using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Core;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation.Implementers;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Error
{
    public class ErrorBehavior : ErrorBehaviorBase
    {
        public ErrorBehavior(string dpseErrorCode) : base(new ErrorHandler(dpseErrorCode))
        {
        }
    }
}