using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Service.Library;
using FXA.DPSET.Framework.Service.Library.Model;
using FXA.DPSET.UnitTests.ServiceInfrastructure;
using NUnit.Framework;

namespace FXA.DPSET.UnitTests
{
    [TestFixture]
    public class ValidateDataAnnotationsServiceBehaviorIntegrationTests : IDisposable
    {
        private const string BaseAddress = "http://localhost:54321";

        private readonly IHealthMonitorService _client;
        private readonly ServiceHost _serviceHost;

        public ValidateDataAnnotationsServiceBehaviorIntegrationTests()
        {
            _serviceHost = ServiceHostUtil.CreateServiceHost(new HealthMonitorService() as IHealthMonitorService, new Uri(BaseAddress), "");
            _client = new ServiceClient<IHealthMonitorService>(BaseAddress).Proxy;
        }

        public void Dispose()
        {
            _serviceHost.Close();
        }

        [Test]
        public void Verify_Correct_Request_Returns_Correct_Id_From_Request_To_Response()
        {
            var id = Guid.NewGuid();
            var request = new HealthMonitorRequest { Id = id, Message = "ABCDEFGH" };
            var response = _client.Post(request);

            Assert.AreEqual(id, response.Id);
        }

        [Test]
        [ExpectedException(typeof(ProtocolException))]
        public void Verify_NULL_Request_Throws_BadRequest_Protocol_Exception()
        {
            var response =_client.Post(null);

            Assert.AreEqual(1, 1);
        }

        [Test]
        [ExpectedException(typeof(ProtocolException))]
        public void Verify_InCorrect_Request_With_NULL_Id_Throws_BadRequest_Protocol_Exception()
        {
            var request = new HealthMonitorRequest { Id = null, Message = "ABCDEFGH" };
            var response = _client.Post(request);

            Assert.AreEqual(1, 1);
        }

        [Test]
        [ExpectedException(typeof(ProtocolException))]
        public void Verify_InCorrect_Request_With_Message_Shorter_Than_5_Throws_BadRequest_Protocol_Exception()
        {
            var id = Guid.NewGuid();
            var request = new HealthMonitorRequest { Id = id, Message = "ABC" };
            var response = _client.Post(request);

            Assert.AreEqual(1, 1);
        }

        [Test]
        [ExpectedException(typeof(ProtocolException))]
        public void Verify_InCorrect_Request_With_NULL_Id_And_Message_Shorter_Than_5_Throws_BadRequest_Protocol_Exception()
        {
            var request = new HealthMonitorRequest { Id = null, Message = "ABC" };
            var response = _client.Post(request);

            Assert.AreEqual(1, 1);
        }
    }
}