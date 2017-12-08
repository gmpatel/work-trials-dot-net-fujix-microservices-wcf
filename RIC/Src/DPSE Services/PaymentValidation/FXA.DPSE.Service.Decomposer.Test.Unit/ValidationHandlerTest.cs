using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.CodelineRules;
using FXA.DPSE.Service.DTO.DuplicateIdentification;
using FXA.DPSE.Service.DTO.LimitChecking;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Decomposer;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.Decomposer.Test.Unit
{
    public class ValidationHandlerTest
    {
        [Fact]
        public void LimitCheckingHandlerShouldSubmitAuditEventByTrackingIdWithAnyProxyResult()
        {
            var loggingMock = new Mock<ILoggingProxy>();
            var auditMock = new Mock<IAuditProxy>();
            auditMock.Setup(e=>e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();
            var httpClientProxy = new Mock<IHttpClientProxy>();
            httpClientProxy
                .Setup(e => e.PostSyncAsJson<TransactionLimitRequest, TransactionLimitResponse>(
                    It.IsAny<string>(), It.IsAny<TransactionLimitRequest>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<int?>()))
                .Returns(new TransactionLimitResponse {Code = "DPSE-4000"});

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ClientSession = new ClientSession(),
                TrackingId = "1000"
            };
            new LimitValidationHandler(loggingMock.Object, auditMock.Object, httpClientProxy.Object).Execute(paymentValidationRequest, string.Empty);
            
            auditMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.AtLeastOnce);
        }

        [Fact]
        public void CodelineHandlerShouldSubmitAuditEventByTrackingIdWithAnyProxyResult()
        {
            var loggingMock = new Mock<ILoggingProxy>();
            var auditMock = new Mock<IAuditProxy>();
            auditMock.Setup(e => e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();
            var httpClientProxy = new Mock<IHttpClientProxy>();
            httpClientProxy
                .Setup(e => e.PostSyncAsJson<CodelineRulesRequest, CodelineRulesResponse>(
                    It.IsAny<string>(), It.IsAny<CodelineRulesRequest>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<int?>()))
                .Returns(new CodelineRulesResponse { Code = "DPSE-8800" });

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ClientSession = new ClientSession(),
                TrackingId = "1000"
            };
            new CodelineValidationHandler(loggingMock.Object, auditMock.Object, httpClientProxy.Object).Execute(paymentValidationRequest, string.Empty);

            auditMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.AtLeastOnce);
        }

        [Fact]
        public void DuplicateHandlerShouldSubmitAuditEventWithAnyProxyResult()
        {
            var loggingMock = new Mock<ILoggingProxy>();
            var auditMock = new Mock<IAuditProxy>();
            auditMock.Setup(e => e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();
            var httpClientProxy = new Mock<IHttpClientProxy>();
            httpClientProxy
                .Setup(e => e.PostSyncAsJson<DuplicateIdentificationRequest, DuplicateIdentificationResponse>(
                    It.IsAny<string>(), It.IsAny<DuplicateIdentificationRequest>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<int?>()))
                .Returns(new DuplicateIdentificationResponse { Code = "DPSE-8900" });

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ClientSession = new ClientSession(),
                TrackingId = "1000"
            };
            new DuplicateValidationHandler(loggingMock.Object, auditMock.Object, httpClientProxy.Object).Execute(paymentValidationRequest, string.Empty);

            auditMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.AtLeastOnce);
        }

        [Fact]
        public void LimitCheckingHandlerShouldSubmitEventLogOnError()
        {
            Mock<ILoggingProxy> loggingMock;
            Mock<IHttpClientProxy> httpClientProxy;
            var auditMock = GetExceptionFlowMock(out loggingMock, out httpClientProxy);

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ClientSession = new ClientSession(),
                TrackingId = "1000"
            };
            new LimitValidationHandler(loggingMock.Object, auditMock.Object, httpClientProxy.Object).Execute(paymentValidationRequest, string.Empty);

            loggingMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.AtLeastOnce);
        }

        [Fact]
        public void CodelineHandlerShouldSubmitEventLogOnError()
        {
            Mock<ILoggingProxy> loggingMock;
            Mock<IHttpClientProxy> httpClientProxy;
            var auditMock = GetExceptionFlowMock(out loggingMock, out httpClientProxy);

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ClientSession = new ClientSession(),
                TrackingId = "1000"
            };
            new CodelineValidationHandler(loggingMock.Object, auditMock.Object, httpClientProxy.Object).Execute(paymentValidationRequest, string.Empty);

            loggingMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.AtLeastOnce);
        }

        [Fact]
        public void DuplicateHandlerShouldSubmitEventLogOnError()
        {
            Mock<ILoggingProxy> loggingMock;
            Mock<IHttpClientProxy> httpClientProxy;
            var auditMock = GetExceptionFlowMock(out loggingMock, out httpClientProxy);

            var paymentValidationRequest = new PaymentValidationRequest
            {
                Cheques = new List<DTO.PaymentValidation.Cheque> { new DTO.PaymentValidation.Cheque() }.ToArray(),
                ClientSession = new ClientSession(),
                TrackingId = "1000"
            };
            new DuplicateValidationHandler(loggingMock.Object, auditMock.Object, httpClientProxy.Object).Execute(paymentValidationRequest, string.Empty);

            loggingMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.AtLeastOnce);
        }

        private static Mock<IAuditProxy> GetExceptionFlowMock(out Mock<ILoggingProxy> loggingMock, out Mock<IHttpClientProxy> httpClientProxy)
        {
            var auditMock = new Mock<IAuditProxy>();
            loggingMock = new Mock<ILoggingProxy>();
            loggingMock.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()))
                .Returns(new BusinessResult()) 
                .Verifiable();

            httpClientProxy = new Mock<IHttpClientProxy>();
            httpClientProxy
                .Setup(e => e.PostSyncAsJson<TransactionLimitRequest, TransactionLimitResponse>(
                    It.IsAny<string>(), It.IsAny<TransactionLimitRequest>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<int?>()))
                .Throws(new Exception());
            return auditMock;
        }
    }
}
