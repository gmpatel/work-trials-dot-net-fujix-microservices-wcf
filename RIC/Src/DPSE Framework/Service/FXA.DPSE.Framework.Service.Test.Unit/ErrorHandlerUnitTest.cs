using System;
using System.ServiceModel;
using FXA.DPSE.Framework.Service.Test.Unit.Core;
using FXA.DPSE.Framework.Service.Test.Unit.ServiceLibrary;
using FXA.DPSE.Framework.Service.Test.Unit.ServiceLibrary.ErrorHandler;
using Xunit;

namespace FXA.DPSE.Framework.Service.Test.Unit
{
    public class ErrorHandlerUnitTest : IDisposable
    {
        //emulates the client proxy
        private readonly IServiceWrapper _clientProxy;

        //the actual instance of the service running under the service host instance
        private readonly IServiceWrapper _serviceWrapper;

        // the servicehost that will contain the WCF instantiation
        private readonly ServiceHost _serviceHost;

        private string _baseAddress = "http://localhost:12345";

        public ErrorHandlerUnitTest()
        {
            //create a service wrapper with an action that throws an exception during execution
            _serviceWrapper = new ServiceWrapper(() =>
            {
                throw new Exception();
            });

            _serviceHost =
                ServiceHostUtil.CreateServiceHost(_serviceWrapper, new Uri(_baseAddress), "");

            ServiceClientTest<IServiceWrapper> client = new ServiceClientTest<IServiceWrapper>(_baseAddress);

            _clientProxy = client.Proxy;
        }

        [Fact]
        public void verify_thrown_exception_is_captured_and_processed_ok()
        {
            //the execute action method contains a single line that throws an exception
            //if the exception is not captured and processed ok, the unit test would fail
            _clientProxy.ExecuteAction();
        }

        public void Dispose()
        {
            _serviceHost.Close();
        }
    }
}