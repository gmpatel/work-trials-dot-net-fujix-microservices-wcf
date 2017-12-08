using System;
using System.Collections.Generic;
using System.Globalization;
using FXA.DPSE.Service.DTO.DipsPayload;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;
using ChequeResponse = FXA.DPSE.Service.DTO.ShadowPost.ChequeResponse;

namespace FXA.DPSE.Service.PaymentInstruction.Test.Unit
{
    public class TraceTrackingActivityRecorder : IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse>
    {
        private readonly List<string> _nodes;
         
        public TraceTrackingActivityRecorder(List<string> nodes)
        {
            _nodes = nodes;
        }

        public PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            _nodes.Add("TraceTracking");
            return new PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse>()
            {
                Response = new ElectronicTraceTrackingResponse()
                {
                    TrackingNumbers = new List<TraceTracking>()
                    {
                        new TraceTracking() {TrackingNumber = "1200"},
                        new TraceTracking() {TrackingNumber = "1300"}
                    }
                },
                IsSucceed = true
            };
        }
    }
    public class PaymentValidationActivityRecorder : IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse>
    {
        private readonly List<string> _nodes;

        public PaymentValidationActivityRecorder(List<string> nodes)
        {
            _nodes = nodes;
        }

        public PaymentInstructionFacadeOut<PaymentValidationResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            _nodes.Add("PaymentValidation");
            return new PaymentInstructionFacadeOut<PaymentValidationResponse>()
            {
                Response = new PaymentValidationResponse(),
                IsSucceed = true
            };
        }
    }
    public class ShadowPostActivityRecorder: IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse>
    {
        private readonly List<string> _nodes;

        public ShadowPostActivityRecorder(List<string> nodes)
        {
            _nodes = nodes;
        }

        public PaymentInstructionFacadeOut<ShadowPostResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            _nodes.Add("ShadowPost");
            return new PaymentInstructionFacadeOut<ShadowPostResponse>()
            {
                Response = new ShadowPostResponse()
                {
                    ChequeResponses = new List<ChequeResponse>()
                    {
                        new ChequeResponse()
                        {
                            ProcessingDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                        }
                    }
                },
                IsSucceed = true
            };
        }
    }
    public class DipsPayloadActivityRecorder : IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse>
    {
        private readonly List<string> _nodes;

        public DipsPayloadActivityRecorder(List<string> nodes)
        {
            _nodes = nodes;
        }

        public PaymentInstructionFacadeOut<DipsPayloadBatchResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            _nodes.Add("DipsPayload");
            return new PaymentInstructionFacadeOut<DipsPayloadBatchResponse>()
            {
                Response = new DipsPayloadBatchResponse(),
                IsSucceed = true
            };
        }
    }
}
