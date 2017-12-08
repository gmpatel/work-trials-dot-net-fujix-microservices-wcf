using System;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;

namespace FXA.DPSE.Framework.Service.Test.Unit.ServiceLibrary.ErrorHandler
{
    [ErrorBehavior("DPSE-500")]
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly Action _customAction;

        public ServiceWrapper(Action customAction)
        {
            _customAction = customAction;
        }

        public void ExecuteAction()
        {
            _customAction();
        }
    }
}
