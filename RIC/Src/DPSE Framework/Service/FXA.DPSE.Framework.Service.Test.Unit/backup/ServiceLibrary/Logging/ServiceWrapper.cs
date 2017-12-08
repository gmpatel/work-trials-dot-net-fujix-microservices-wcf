using System;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;

namespace FXA.DPSE.Framework.Service.Test.Unit.ServiceLibrary.Logging
{
    [Logging]
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly Action _customAction;
        private readonly Action _callLogging;

        public ServiceWrapper(Action customAction, Action callLogging)
        {
            _customAction = customAction;
            _callLogging = callLogging;
        }

        public void MethodInterceptedByLogging()
        {
            _customAction();
        }

        [IgnoreLogging]
        public void MethodIgnoredByLogging()
        {
            _customAction();
        }

        public void MethodThatCallsLoggingServiceInternally()
        {
            _callLogging();
        }
    }
}