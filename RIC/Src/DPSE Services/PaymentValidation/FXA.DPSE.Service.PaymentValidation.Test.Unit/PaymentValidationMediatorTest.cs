using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.LimitChecking;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Common.Configuration;
using FXA.DPSE.Service.PaymentValidation.Core;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;
using Xunit;
using Moq;
using Cheque = FXA.DPSE.Service.DTO.LimitChecking.Cheque;

namespace FXA.DPSE.Service.PaymentValidation.Test.Unit
{
    public class PaymentValidationMediatorTest
    {
        [Fact]
        public void ShouldExecuteAllValidationHandlersWithDefaultValidationMode()
        {
            var validationConfiguration = GetValidationConfiguration();

            Mock<IValidationHandler> secondHandler;
            Mock<IValidationComposite> validationComposite;
            var firstHandler = GetValidationComposite(out secondHandler, out validationComposite);

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ValidationMode = "Default",
                ClientSession = new ClientSession()
            };

            new PaymentValidationMediator(validationConfiguration.Object, validationComposite.Object)
                .ValidatePayload(paymentValidationRequest);

            firstHandler.Verify(e => e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>()), Times.Once);
            secondHandler.Verify(e => e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldExecuteSingleValidationHandlerWithValidationMode()
        {
            var validationConfiguration = GetValidationConfiguration();

            Mock<IValidationHandler> secondHandler;
            Mock<IValidationComposite> validationComposite;
            var firstHandler = GetValidationComposite(out secondHandler, out validationComposite);

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ValidationMode = "Limit",
                ClientSession = new ClientSession()
            };

            new PaymentValidationMediator(validationConfiguration.Object, validationComposite.Object)
                .ValidatePayload(paymentValidationRequest);

            firstHandler.Verify(e => e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>()), Times.Once);
            secondHandler.Verify(e => e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ShouldNotBubbleUpAnyExceptionFromValidationHandler()
        {
            var validationConfiguration = new Mock<IPaymentValidationServiceConfiguration>();
            validationConfiguration.Setup(e => e.RoutingSlip).Returns(new RoutingSlip
            {
                ValidationServices = new List<WorkflowValidationService>
                {
                    new WorkflowValidationService {Name = "Limit", Order = 1}
                }
            });

            var httpClientProxyMock = new Mock<IHttpClientProxy>();
            httpClientProxyMock.Setup(e => e.PostSyncAsJson<TransactionLimitRequest, TransactionLimitResponse>(It.IsAny<string>(),
                        It.IsAny<TransactionLimitRequest>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<int?>())).Throws(new Exception("ProxyException"));

            var limitValidationHandlerFake = new LimitValidationHandler(new LoggingProxyFake(), new AuditProxyFake(), httpClientProxyMock.Object);

            var validationComposite = new Mock<IValidationComposite>();
            validationComposite.Setup(e => e.Services).Returns(new Dictionary<string, IValidationHandler>()
            {
                {"Limit", limitValidationHandlerFake},
            });

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> {new DTO.PaymentValidation.Cheque()}.ToArray(),
                ValidationMode = "Default",
                ClientSession = new ClientSession()
            };

            var validationMedidator = new PaymentValidationMediator(validationConfiguration.Object, validationComposite.Object);
            var validationResponse = validationMedidator.ValidatePayload(paymentValidationRequest);

            Assert.NotNull(validationResponse);
            Assert.False(validationResponse.IsSucceed);
            Assert.True(validationResponse.InternalError);
        }

        [Fact]
        public void ShouldNotExecutePendingValidationHandlerWhenReceivedAnyError()
        {
            var validationConfiguration = GetValidationConfiguration();

            var httpClientProxyMock = new Mock<IHttpClientProxy>();
            httpClientProxyMock.Setup(e => e.PostSyncAsJson<TransactionLimitRequest, TransactionLimitResponse>(It.IsAny<string>(),
                        It.IsAny<TransactionLimitRequest>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<int?>())).Throws(new Exception("ProxyException"));

            var limitValidationHandlerFake = new LimitValidationHandler(new LoggingProxyFake(), new AuditProxyFake(), httpClientProxyMock.Object);

            var codelineHandlerMock = new Mock<IValidationHandler>();
            codelineHandlerMock.Setup(e=>e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>())).Verifiable();

            var validationComposite = new Mock<IValidationComposite>();
            validationComposite.Setup(e => e.Services).Returns(new Dictionary<string, IValidationHandler>()
            {
                {"Limit", limitValidationHandlerFake},
                {"Codeline", codelineHandlerMock.Object}
            });

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ValidationMode = "Default",
                ClientSession = new ClientSession()
            };

            var validationMedidator = new PaymentValidationMediator(validationConfiguration.Object, validationComposite.Object);
            var validationResponse = validationMedidator.ValidatePayload(paymentValidationRequest);

            Assert.NotNull(validationResponse);
            Assert.False(validationResponse.IsSucceed);
            codelineHandlerMock.Verify(e=>e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>()),Times.Never);
        }

        private static Mock<IValidationHandler> GetValidationComposite(out Mock<IValidationHandler> secondHandler, out Mock<IValidationComposite> validationComposite)
        {
            var firstHandler = new Mock<IValidationHandler>();
            firstHandler.Setup(e => e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>()))
                .Returns(new ValidationResponse() {IsSucceed = true})
                .Verifiable();

            secondHandler = new Mock<IValidationHandler>();
            secondHandler.Setup(e => e.Execute(It.IsAny<PaymentValidationRequest>(), It.IsAny<string>()))
                .Returns(new ValidationResponse() {IsSucceed = true})
                .Verifiable();

            validationComposite = new Mock<IValidationComposite>();
            validationComposite.Setup(e => e.Services).Returns(new Dictionary<string, IValidationHandler>()
            {
                {"Limit", firstHandler.Object},
                {"Codeline", secondHandler.Object}
            });
            return firstHandler;
        }

        private static Mock<IPaymentValidationServiceConfiguration> GetValidationConfiguration()
        {
            var validationConfiguration = new Mock<IPaymentValidationServiceConfiguration>();
            validationConfiguration.Setup(e => e.RoutingSlip).Returns(new RoutingSlip
            {
                ValidationServices = new List<WorkflowValidationService>
                {
                    new WorkflowValidationService {Name = "Limit", Order = 1},
                    new WorkflowValidationService {Name = "Codeline", Order = 2}
                }
            });
            return validationConfiguration;
        }
    }
}
