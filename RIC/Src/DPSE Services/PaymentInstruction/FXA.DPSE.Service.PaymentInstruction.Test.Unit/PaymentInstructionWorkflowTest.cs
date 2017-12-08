using System;
using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Service.DTO.DipsPayload;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.PaymentInstruction.Business;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;
using Moq;
using Xunit;
using AccountDetails = FXA.DPSE.Service.DTO.PaymentInstruction.AccountDetails;
using Session = FXA.DPSE.Service.DTO.PaymentInstruction.Session;

namespace FXA.DPSE.Service.PaymentInstruction.Test.Unit
{
    public class PaymentInstructionWorkflowTest
    {
        [Fact]
        public void WorkflowShouldCallServicesInParticularOrder()
        {
            var paymentProcessorMock = new Mock<IPaymentInstructionBusiness>();
            paymentProcessorMock.Setup(e => e.AssignTrackingIdentifiers(It.IsAny<PaymentInstructionBusinessData>(), It.IsAny<List<string>>())).Returns(new TrackingIdentifierResult());
            paymentProcessorMock.Setup(e => e.StorePaymentInstruction(It.IsAny<PaymentInstructionBusinessData>(), It.IsAny<TrackingIdentifierResult>())).Returns(new PaymentInstructionStoreResult());
            paymentProcessorMock.Setup(e => e.UpdatePaymentInstructionWithShadowPost(It.IsAny<long>(), It.IsAny<DateTime?>())).Returns(new BusinessResult());

            var orchestrationServiceNodes = new List<string>();
            var mediatorResult = new PaymentInstructionWorkflow(
                new TraceTrackingActivityRecorder(orchestrationServiceNodes),
                new DipsPayloadActivityRecorder(orchestrationServiceNodes),
                new PaymentValidationActivityRecorder(orchestrationServiceNodes),
                new ShadowPostActivityRecorder(orchestrationServiceNodes),
                paymentProcessorMock.Object).Run(new PaymentInstructionRequestWrapper
                {
                    Content = new PaymentInstructionRequest
                    {
                        ClientSession = new Session(),
                        DepositingAccountDetails = new AccountDetails(),
                        PostingCheques = new List<PostingCheque>(),
                        NonPostingCheques = new List<NonPostingCheque>(),
                        Notifications = new List<Notification>()
                    }
                });

            Assert.Equal(4, orchestrationServiceNodes.Count);
            Assert.Equal("TraceTracking", orchestrationServiceNodes[0]);
            Assert.Equal("PaymentValidation", orchestrationServiceNodes[1]);
            Assert.Equal("ShadowPost", orchestrationServiceNodes[2]);
            Assert.Equal("DipsPayload", orchestrationServiceNodes[3]);
            Assert.True(mediatorResult.IsSucceed);
        }

        [Fact]
        public void WorkflowShouldStopProcessingOnTraceTrackingFacadeError()
        {
            var paymentProcessorMock = new Mock<IPaymentInstructionBusiness>();

            var dipsPayloadMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse>>();
            var shadowPostMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse>>();
            var validationMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse>>();

            dipsPayloadMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Verifiable();
            shadowPostMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Verifiable();
            validationMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Verifiable();

            var traceTrackingMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse>>();
            traceTrackingMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Returns(new PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse>());

            var mediatorResult = new PaymentInstructionWorkflow(traceTrackingMock.Object, dipsPayloadMock.Object,
                validationMock.Object, shadowPostMock.Object, paymentProcessorMock.Object).Run(new PaymentInstructionRequestWrapper()
                {
                    Content = new PaymentInstructionRequest()
                    {
                        DepositingAccountDetails = new AccountDetails()
                        {
                            Names = new List<AccountName>()
                        },
                        PostingCheques = new List<PostingCheque>(),
                        ClientSession = new Session(),
                        Notifications = new List<Notification>()
                    }
                });

            dipsPayloadMock.Verify(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()), Times.Never());
            shadowPostMock.Verify(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()), Times.Never());
            validationMock.Verify(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()), Times.Never());

            Assert.False(mediatorResult.IsSucceed);
        }

        [Fact]
        public void WorkflowShouldStopProcessingOnPaymentValidationFacadeError()
        {
            var paymentProcessorMock = new Mock<IPaymentInstructionBusiness>();
            paymentProcessorMock.Setup(e => e.AssignTrackingIdentifiers(It.IsAny<PaymentInstructionBusinessData>(), It.IsAny<List<string>>())).Returns(new TrackingIdentifierResult());

            var dipsPayloadMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse>>();
            dipsPayloadMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Verifiable();

            var shadowPostMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse>>();
            shadowPostMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Verifiable();

            var validationMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse>>();
            validationMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()))
                .Returns(new PaymentInstructionFacadeOut<PaymentValidationResponse>()
                {
                    Response = new PaymentValidationResponse()
                });

            var traceTrackingMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse>>();
            traceTrackingMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()))
                .Returns(new PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse>
                {
                    IsSucceed = true,
                    Response = new ElectronicTraceTrackingResponse
                    {
                        TrackingNumbers = new List<TraceTracking>()
                        {
                            new TraceTracking(){TrackingNumber = "1100"},
                            new TraceTracking(){TrackingNumber = "2200"}
                        }
                    }
                }).Verifiable();

            

            var mediatorResult = new PaymentInstructionWorkflow(traceTrackingMock.Object, dipsPayloadMock.Object,
                validationMock.Object, shadowPostMock.Object, paymentProcessorMock.Object).Run(new PaymentInstructionRequestWrapper()
                {
                    Content = new PaymentInstructionRequest()
                    {
                        DepositingAccountDetails = new AccountDetails()
                        {
                            Names = new List<AccountName>()
                        },
                        PostingCheques = new List<PostingCheque>(),
                        ClientSession = new Session(),
                        Notifications = new List<Notification>()
                    }
                });

            dipsPayloadMock.Verify(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()), Times.Never());
            shadowPostMock.Verify(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()), Times.Never());
            Assert.False(mediatorResult.IsSucceed);
        }

        [Fact]
        public void WorkflowShouldStopProcessingOnShadowPostFacadeError()
        {
            var paymentProcessorMock = new Mock<IPaymentInstructionBusiness>();
            paymentProcessorMock.Setup(e => e.AssignTrackingIdentifiers(It.IsAny<PaymentInstructionBusinessData>(), It.IsAny<List<string>>())).Returns(new TrackingIdentifierResult());
            paymentProcessorMock.Setup(e => e.StorePaymentInstruction(It.IsAny<PaymentInstructionBusinessData>(), It.IsAny<TrackingIdentifierResult>())).Returns(new PaymentInstructionStoreResult());

            var dipsPayloadMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse>>();
            dipsPayloadMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Verifiable();

            var validationMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse>>();
            validationMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()))
                .Returns(new PaymentInstructionFacadeOut<PaymentValidationResponse>()
                {
                    IsSucceed = true,
                    Response = new PaymentValidationResponse()
                });

            var traceTrackingMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse>>();
            traceTrackingMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()))
                .Returns(new PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse>
                {
                    IsSucceed = true,
                    Response = new ElectronicTraceTrackingResponse
                    {
                        TrackingNumbers = new List<TraceTracking>()
                        {
                            new TraceTracking(){TrackingNumber = "1100"},
                            new TraceTracking(){TrackingNumber = "2200"}
                        }
                    }
                }).Verifiable();

            var shadowPostMock = new Mock<IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse>>();
            shadowPostMock.Setup(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>())).Returns(new PaymentInstructionFacadeOut<ShadowPostResponse>()).Verifiable();

            var mediatorResult = new PaymentInstructionWorkflow(traceTrackingMock.Object, dipsPayloadMock.Object,
                validationMock.Object, shadowPostMock.Object, paymentProcessorMock.Object).Run(new PaymentInstructionRequestWrapper()
                {
                    Content = new PaymentInstructionRequest()
                    {
                        DepositingAccountDetails = new AccountDetails()
                        {
                            Names = new List<AccountName>()
                        },
                        PostingCheques = new List<PostingCheque>(),
                        ClientSession = new Session(),
                        Notifications = new List<Notification>()
                    }
                });

            dipsPayloadMock.Verify(e => e.Call(It.IsAny<PaymentInstructionFacadeIn<TrackingIdentifierResult>>()), Times.Never());
            Assert.False(mediatorResult.IsSucceed);
        }

        [Fact]
        public void WorkflowShouldRollbackStoredPaymentInstructionOnShadowPostError()
        {
            //Delete payment instruction records by instruction Id
        }

        [Fact]
        public void WorkflowShouldRollbackShadowPostAndStoredPaymentInstructionOnDipsPayloadError()
        {
            //DipsPayload.DeleteBatch() (files, metadata)
            //Delete payment instruction records by instruction Id
        }
    }
}
