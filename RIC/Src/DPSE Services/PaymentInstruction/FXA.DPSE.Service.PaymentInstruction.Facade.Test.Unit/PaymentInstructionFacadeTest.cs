using System;
using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.DipsPayload;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;
using Moq;
using Xunit;
using AccountDetails = FXA.DPSE.Service.DTO.PaymentInstruction.AccountDetails;
using Session = FXA.DPSE.Service.DTO.PaymentInstruction.Session;

namespace FXA.DPSE.Service.PaymentInstruction.Facade.Test.Unit
{
    public class PaymentInstructionFacadeTest
    {
        [Fact]
        public void DipsPayloadFacadeMustSubmitAuditEventAfterResponseReceived()
        {
            var paymentInstructionFacadeIn = new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                PaymentInstructionRequest = new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest()
                    {
                        ClientSession = new Session()
                    }
                },
                Data = new TrackingIdentifierResult
                {
                    ForCredit = "6600"
                }
            };
            var httpClientMock = new Mock<IHttpClientProxy>();
            httpClientMock.Setup(e => e.PostSyncAsJson<DipsPayloadBatchRequest, DipsPayloadBatchResponse>(It.IsAny<string>(), It.IsAny<DipsPayloadBatchRequest>(), null, It.IsAny<int?>()))
                .Returns(new DipsPayloadBatchResponse() { Code = "DPSE-6000" });
            
            var loggingProxyMock = new Mock<ILoggingProxy>();
            
            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.DipsPayloadService).Returns(new DipsPayloadServiceElement() { Url = "http://" });

            var auditProxyMock = new Mock<IAuditProxy>();
            auditProxyMock.Setup(e=>e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var dipsPayloadFacade = new DipsPayloadFacade(configurationMock.Object, loggingProxyMock.Object, auditProxyMock.Object, httpClientMock.Object);
            var facadeResult = dipsPayloadFacade.Call(paymentInstructionFacadeIn);

            auditProxyMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.Once);
            Assert.True(facadeResult.IsSucceed);
        }

        [Fact]
        public void ShadowPostFacadeMustSubmitAuditEventAfterResponseReceived()
        {
            var paymentInstructionFacadeIn = new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                PaymentInstructionRequest = new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest()
                    {
                        ClientSession = new Session(),
                        DepositingAccountDetails = new AccountDetails()
                        {
                            Names = new List<AccountName>()
                        },
                        PostingCheques = new List<PostingCheque>()
                    }
                },
                Data = new TrackingIdentifierResult
                {
                    ForCredit = "6600"
                }
            };
            var httpClientMock = new Mock<IHttpClientProxy>();
            httpClientMock.Setup(e => e.PostSyncAsJson<ShadowPostRequest, ShadowPostResponse>(It.IsAny<string>(), It.IsAny<ShadowPostRequest>(), null, It.IsAny<int?>()))
                .Returns(new ShadowPostResponse() { Code = "DPSE-7000" });

            var loggingProxyMock = new Mock<ILoggingProxy>();

            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.ShadowPostService).Returns(new ShadowPostServiceElement() { Url = "http://" });

            var auditProxyMock = new Mock<IAuditProxy>();
            auditProxyMock.Setup(e => e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var shadowPostFacade = new ShadowPostFacade(configurationMock.Object, loggingProxyMock.Object, auditProxyMock.Object, httpClientMock.Object);
            var facadeResult = shadowPostFacade.Call(paymentInstructionFacadeIn);

            auditProxyMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.Once);
            Assert.True(facadeResult.IsSucceed);
        }

        [Fact]
        public void TraceTrackingFacadeMustSubmitAuditEventAfterResponseReceived()
        {
            //Be completed later.
        }

        [Fact]
        public void ValidationFacadeMustSubmitAuditEventAfterResponseReceived()
        {
            var paymentInstructionFacadeIn = new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                PaymentInstructionRequest = new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest()
                    {
                        ClientSession = new Session(),
                        DepositingAccountDetails = new AccountDetails()
                        {
                            Names = new List<AccountName>()
                        },
                        PostingCheques =  new List<PostingCheque>()
                    }
                },
                Data = new TrackingIdentifierResult
                {
                    ForCredit = "6600"
                }
            };
            var httpClientMock = new Mock<IHttpClientProxy>();
            httpClientMock.Setup(e => e.PostSyncAsJson<PaymentValidationRequest, PaymentValidationResponse>(It.IsAny<string>(),
                        It.IsAny<PaymentValidationRequest>(), null, It.IsAny<int?>())).Returns(new PaymentValidationResponse() { Code = "DPSE-8000" });
            var loggingProxyMock = new Mock<ILoggingProxy>();
            loggingProxyMock.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.PaymentValidationService)
                .Returns(new PaymentValidationServiceElement() { Url = "http://" });


            var auditProxyMock = new Mock<IAuditProxy>();
            auditProxyMock.Setup(e => e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var validationFacade = new PaymentValidationFacade(configurationMock.Object, loggingProxyMock.Object, auditProxyMock.Object, httpClientMock.Object);
            var facadeResult = validationFacade.Call(paymentInstructionFacadeIn);

            auditProxyMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.Once);
            Assert.True(facadeResult.IsSucceed);
        }

        [Fact]
        public void DipsPayloadFacadeMustSubmitEventLogOnError()
        {
            var paymentInstructionFacadeIn = new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                PaymentInstructionRequest = new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest()
                    {
                        ClientSession = new Session()
                    }
                },
                Data = new TrackingIdentifierResult
                {
                    ForCredit = "6600"
                }
            };
            var httpClientMock = new Mock<IHttpClientProxy>();
            httpClientMock.Setup(e => e.PostSyncAsJson<DipsPayloadBatchRequest, DipsPayloadBatchResponse>(It.IsAny<string>(),
                        It.IsAny<DipsPayloadBatchRequest>(), null, It.IsAny<int?>())).Throws(new Exception("AnyException"));
            var loggingProxyMock = new Mock<ILoggingProxy>();
            loggingProxyMock.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.DipsPayloadService)
                .Returns(new DipsPayloadServiceElement() { Url = "http://" });
            var dipsPayloadFacade = new DipsPayloadFacade(configurationMock.Object, loggingProxyMock.Object, new AuditProxyFake(), httpClientMock.Object);
            var facadeResult = dipsPayloadFacade.Call(paymentInstructionFacadeIn);

            loggingProxyMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.Once);
            Assert.False(facadeResult.IsSucceed);
            Assert.True(facadeResult.FacadeErrorType == FacadeErrorType.InternalFailure);
        }

        [Fact]
        public void ShadowPostFacadeMustSubmitEventLogtOnError()
        {
            var paymentInstructionFacadeIn = new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                PaymentInstructionRequest = new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest()
                    {
                        ClientSession = new Session()
                    }
                },
                Data = new TrackingIdentifierResult
                {
                    ForCredit = "6600"
                }
            };
            var httpClientMock = new Mock<IHttpClientProxy>();
            httpClientMock.Setup(e => e.PostSyncAsJson<ShadowPostRequest, ShadowPostResponse>(It.IsAny<string>(),
                        It.IsAny<ShadowPostRequest>(), null, It.IsAny<int?>())).Throws(new Exception("AnyException"));
            var loggingProxyMock = new Mock<ILoggingProxy>();
            loggingProxyMock.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.ShadowPostService)
                .Returns(new ShadowPostServiceElement() { Url = "http://" });
            var shadowPostFacade = new ShadowPostFacade(configurationMock.Object, loggingProxyMock.Object, new AuditProxyFake(), httpClientMock.Object);
            var facadeResult = shadowPostFacade.Call(paymentInstructionFacadeIn);

            loggingProxyMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.Once);
            Assert.False(facadeResult.IsSucceed);
            Assert.True(facadeResult.FacadeErrorType == FacadeErrorType.InternalFailure);
        }

        [Fact]
        public void TraceTrackingFacadeMustSubmitEventLogOnError()
        {
            var paymentInstructionFacadeIn = new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                PaymentInstructionRequest = new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest()
                    {
                        ClientSession = new Session()
                    }
                },
                Data = new TrackingIdentifierResult
                {
                    ForCredit = "6600"
                }
            };
            var httpClientMock = new Mock<IHttpClientProxy>();
            httpClientMock.Setup(e => e.PostSyncAsJson<ElectronicTraceTrackingRequest, ElectronicTraceTrackingResponse>(It.IsAny<string>(),
                        It.IsAny<ElectronicTraceTrackingRequest>(), null, It.IsAny<int?>())).Throws(new Exception("AnyException"));
            var loggingProxyMock = new Mock<ILoggingProxy>();
            loggingProxyMock.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.TraceTrackingService)
                .Returns(new TraceTrackingServiceElement() { Url = "http://" });
            var traceTrackingFacade = new ShadowPostFacade(configurationMock.Object, loggingProxyMock.Object, new AuditProxyFake(), httpClientMock.Object);
            var facadeResult = traceTrackingFacade.Call(paymentInstructionFacadeIn);

            loggingProxyMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.Once);
            Assert.False(facadeResult.IsSucceed);
            Assert.True(facadeResult.FacadeErrorType == FacadeErrorType.InternalFailure);
        }

        [Fact]
        public void ValidationFacadeMustSubmitEventLogOnError()
        {
            var paymentInstructionFacadeIn = new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                PaymentInstructionRequest = new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest()
                    {
                        ClientSession = new Session()
                    }
                },
                Data = new TrackingIdentifierResult
                {
                    ForCredit = "6600"
                }
            };
            var httpClientMock = new Mock<IHttpClientProxy>();
            httpClientMock.Setup(e => e.PostSyncAsJson<PaymentValidationRequest, PaymentValidationResponse>(It.IsAny<string>(),
                        It.IsAny<PaymentValidationRequest>(), null, It.IsAny<int?>())).Throws(new Exception("AnyException"));
            var loggingProxyMock = new Mock<ILoggingProxy>();
            loggingProxyMock.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.PaymentValidationService)
                .Returns(new PaymentValidationServiceElement(){ Url = "http://" });
            var validationFacade = new PaymentValidationFacade(configurationMock.Object, loggingProxyMock.Object, new AuditProxyFake(), httpClientMock.Object);
            var facadeResult = validationFacade.Call(paymentInstructionFacadeIn);

            loggingProxyMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.Once);
            Assert.False(facadeResult.IsSucceed);
            Assert.True(facadeResult.FacadeErrorType == FacadeErrorType.InternalFailure);
        }
    }
}
