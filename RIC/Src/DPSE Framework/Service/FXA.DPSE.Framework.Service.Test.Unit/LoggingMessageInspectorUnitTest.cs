//using System;
//using System.ServiceModel;
//using FXA.DPSE.Framework.Service.Test.Unit.Core;
//using FXA.DPSE.Framework.Service.Test.Unit.ServiceLibrary.Logging;
//using FXA.DPSE.Service.DTO.Logging;
//using FXA.DPSE.Service.Logging;
//using FXA.DPSE.Service.Logging.Business;
//using Moq;
//using Xunit;

//namespace FXA.DPSE.Framework.Service.Test.Unit
//{
//    public class LoggingMessageInspectorUnitTest : IDisposable
//    {
//        private IBusinessLogger businessLogger;
//        private ILoggingBusinessService _loggingBusinessService;

//        private readonly Mock<IBusinessLogger> _logWriter;

//        private readonly ILoggingService _clientProxyLoggingServiceHost;
//        private readonly IServiceWrapper _clientProxyDummyServiceHost;
//        //private readonly ILoggingService _clientProxyAuditServiceHost;

//        private readonly ILoggingService _loggingService;

//        private readonly IServiceWrapper _serviceWrapper;

//        //service hosts
//        private readonly ServiceHost _dummyServiceHost, _loggingServiceHost;

//        //base addresses
//        private string _dummyBaseAddress = "http://localhost:1234";
//        private string _loggingServiceBaseAddress = "http://localhost:6789";

//        public LoggingMessageInspectorUnitTest()
//        {
//            /******************************************/
//            /** WCF call flow:
//            /** _clientProxyDummyServiceHost  ->  _dummyServiceHost  ->  _loggingServiceHost
//            /**
//            /** service call flows:
//            /** 
//            /******************************************/

//            _logWriter = new Mock<IBusinessLogger>();

//            _logWriter.Setup(writer => writer.Log(
//                It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<object>()));

//            _loggingBusinessService = new LoggingBusinessService(_logWriter.Object);
//            _loggingService = new LoggingService(_loggingBusinessService);
//            _loggingServiceHost = 
//                ServiceHostUtil.CreateServiceHost(_loggingService, new Uri(_loggingServiceBaseAddress),"");

//            ServiceClientTest<ILoggingService> clientLoggingServiceHost = 
//                new ServiceClientTest<ILoggingService>(_loggingServiceBaseAddress);

//            _clientProxyLoggingServiceHost = clientLoggingServiceHost.Proxy;

//            _serviceWrapper = new ServiceWrapper(() =>
//            {
                
//            }, () =>
//            {
//                LoggingRequest loggingRequest = 
//                    new LoggingRequest(
//                        Guid.NewGuid().ToString(),
//                        "the name",
//                        "the description",
//                        "the channel typ",
//                        Guid.NewGuid(),
//                        "the machine name",
//                        "the service name",
//                        "the operation name");
//                _clientProxyLoggingServiceHost.StoreEvent(loggingRequest);
//            });

//            _dummyServiceHost = 
//                ServiceHostUtil.CreateServiceHost(_serviceWrapper, new Uri(_dummyBaseAddress), "");
            
//            ServiceClientTest<IServiceWrapper> clientDummyServiceHost = 
//                new ServiceClientTest<IServiceWrapper>(_dummyBaseAddress);

//            _clientProxyDummyServiceHost = clientDummyServiceHost.Proxy;
//        }

//        [Fact]
//        public void verify_dummy_service_gets_intercepted_by_loggingInterceptor()
//        {
//            _clientProxyDummyServiceHost.MethodInterceptedByLogging();

//            _logWriter.Verify(logger => logger.Log(It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<object>()));
//        }

//        [Fact]
//        public void verify_method_gets_ignored_by_loggingInterceptor()
//        {
//            _clientProxyDummyServiceHost.MethodIgnoredByLogging();

//            _logWriter.Verify(logger => logger.Log(It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<object>()));
//        }

//        [Fact]
//        public void verify_direct_call_to_loggingservice()
//        {
//            _clientProxyDummyServiceHost.MethodThatCallsLoggingServiceInternally();

//            _logWriter.Verify(logger => logger.Log(It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<object>()));
//        }

//        public void Dispose()
//        {
//            _dummyServiceHost.Close();
//            _loggingServiceHost.Close();
//        }
//    }
//}