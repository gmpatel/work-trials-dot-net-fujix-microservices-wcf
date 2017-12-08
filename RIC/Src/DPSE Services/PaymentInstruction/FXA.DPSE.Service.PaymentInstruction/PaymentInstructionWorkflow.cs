using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DTO.DipsPayload;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.PaymentInstruction.Business;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common.Converters;
using FXA.DPSE.Service.PaymentInstruction.Core;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction
{
    //TODO: What is the Rollback scenario when something goes wrong in the middle ?!

    public class PaymentInstructionWorkflow : IPaymentInstructionWorkflow
    {
        private readonly IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse> _traceTrackingFacade;
        private readonly IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse> _dipsPayloadFacade;
        private readonly IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse> _paymentValidationFacade;
        private readonly IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse> _shadowPostFacade;
        private readonly IPaymentInstructionBusiness _paymentInstructionProcessor;

        public PaymentInstructionWorkflow(
             IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse> traceTrackingFacade
            , IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse> dipsPayloadFacade
            , IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse> paymentValidationFacade
            , IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse> shadowPostFacade
            , IPaymentInstructionBusiness paymentInstructionProcessor)
        {
            _dipsPayloadFacade = dipsPayloadFacade;
            _paymentValidationFacade = paymentValidationFacade;
            _shadowPostFacade = shadowPostFacade;
            _traceTrackingFacade = traceTrackingFacade;
            _paymentInstructionProcessor = paymentInstructionProcessor;
        }

        public PaymentInstructionFacadeOut<PaymentInstructionResponse> Run(PaymentInstructionRequestWrapper paymentInstructionRequestWrapper)
        {
            List<string> generatedTrackindIds;
            PaymentInstructionFacadeOut<PaymentInstructionResponse> traceTrackingFailureHandlerResult;
            if (CallTraceTrackingService(paymentInstructionRequestWrapper, out generatedTrackindIds, out traceTrackingFailureHandlerResult)) 
                return traceTrackingFailureHandlerResult;
           
            var trackingIdentifierAssignmentResult = _paymentInstructionProcessor.AssignTrackingIdentifiers(paymentInstructionRequestWrapper.Content.GetPaymentInstructionBusinessData(), generatedTrackindIds);

            PaymentInstructionFacadeOut<PaymentInstructionResponse> paymentValidationFailureHandlerResult;
            if (CallPaymentValidationService(paymentInstructionRequestWrapper, trackingIdentifierAssignmentResult, out paymentValidationFailureHandlerResult)) 
                return paymentValidationFailureHandlerResult;

            var result = _paymentInstructionProcessor.StorePaymentInstruction(paymentInstructionRequestWrapper.Content.GetPaymentInstructionBusinessData(), trackingIdentifierAssignmentResult);
            
            if (result.HasException)
            {
                var response = ResponseHelper.GetBasicPaymentInstructionResponse(paymentInstructionRequestWrapper.Content);
                response.BusinessErrorCode = result.BusinessException.ErrorCode;
                response.FacadeErrorType = (result.BusinessException.ExceptionType != DpseBusinessExceptionType.ApplicationException)
                    ? FacadeErrorType.BusinessFailure
                    : FacadeErrorType.InternalFailure;
                return response;
            }

            DateTime? processingDate;
            PaymentInstructionFacadeOut<PaymentInstructionResponse> shadowPostFailureHandlerResult;
            if (CallShadowPostService(paymentInstructionRequestWrapper, trackingIdentifierAssignmentResult, out processingDate, out shadowPostFailureHandlerResult))
                return shadowPostFailureHandlerResult;

            var updateProcessingDateResult = _paymentInstructionProcessor.UpdatePaymentInstructionWithShadowPost(result.PaymentInstructionId, processingDate);

            if (updateProcessingDateResult.HasException)
            {
                var response = ResponseHelper.GetBasicPaymentInstructionResponse(paymentInstructionRequestWrapper.Content);
                response.BusinessErrorCode = updateProcessingDateResult.BusinessException.ErrorCode;
                response.FacadeErrorType = (updateProcessingDateResult.BusinessException.ExceptionType != DpseBusinessExceptionType.ApplicationException)
                    ? FacadeErrorType.BusinessFailure
                    : FacadeErrorType.InternalFailure;
                return response;
            }
            PaymentInstructionFacadeOut<PaymentInstructionResponse> dipsPayloadFailureHandlerResult;
            if (CallDipsPayload(paymentInstructionRequestWrapper, trackingIdentifierAssignmentResult, out dipsPayloadFailureHandlerResult)) return dipsPayloadFailureHandlerResult;

            var instructionResult = ResponseHelper.GetBasicPaymentInstructionResponse(paymentInstructionRequestWrapper.Content);
            instructionResult.Response.ResultStatus = "Success";
            instructionResult.Response.TrackingId = trackingIdentifierAssignmentResult.ForCredit;
            instructionResult.IsSucceed = true;
            return instructionResult;
        }

        private bool CallDipsPayload(PaymentInstructionRequestWrapper paymentInstructionRequestWrapper,
            TrackingIdentifierResult trackingIdentifierAssignmentResult,
            out PaymentInstructionFacadeOut<PaymentInstructionResponse> handleDipsPayloadFailure)
        {
            handleDipsPayloadFailure = null;
            var dipsPayloadFacadeResult = _dipsPayloadFacade.Call(new PaymentInstructionFacadeIn<TrackingIdentifierResult>()
            {
                Data = trackingIdentifierAssignmentResult,
                PaymentInstructionRequest = paymentInstructionRequestWrapper
            });
            if (dipsPayloadFacadeResult.IsSucceed) return false;
            handleDipsPayloadFailure = FacadeFailureResponseHandler.HandleDipsPayloadFailure(dipsPayloadFacadeResult,
                paymentInstructionRequestWrapper.Content, trackingIdentifierAssignmentResult);
            return true;
        }

        private bool CallShadowPostService(PaymentInstructionRequestWrapper paymentInstructionRequestWrapper,
            TrackingIdentifierResult trackingIdentifierAssignmentResult, out DateTime? processingDate, out PaymentInstructionFacadeOut<PaymentInstructionResponse> handleShadowPostFailure)
        {
            handleShadowPostFailure = null;
            processingDate = null;
            var shadowPostFacadeResult = _shadowPostFacade.Call(new PaymentInstructionFacadeIn<TrackingIdentifierResult>()
            {
                Data = trackingIdentifierAssignmentResult,
                PaymentInstructionRequest = paymentInstructionRequestWrapper
            });
            if (!shadowPostFacadeResult.IsSucceed)
            {
                handleShadowPostFailure = FacadeFailureResponseHandler.HandleShadowPostFailure(shadowPostFacadeResult,
                    paymentInstructionRequestWrapper.Content, trackingIdentifierAssignmentResult);
                return true;
            }
            //Assuming there is only one cheque in the payload.(i.e. ChequeResponses.First())
            DateTime chequeProcessingDate;
            if(DateTime.TryParse(shadowPostFacadeResult.Response.ChequeResponses.First().ProcessingDate, out chequeProcessingDate)) 
                processingDate = chequeProcessingDate;
            return false;
        }

        private bool CallPaymentValidationService(PaymentInstructionRequestWrapper paymentInstructionRequestWrapper,
            TrackingIdentifierResult trackingIdentifierAssignmentResult,
            out PaymentInstructionFacadeOut<PaymentInstructionResponse> handlePaymentValidationFailure)
        {
            handlePaymentValidationFailure = null;
            var paymentValidationFacadeResult =
                _paymentValidationFacade.Call(new PaymentInstructionFacadeIn<TrackingIdentifierResult>()
                {
                    Data = trackingIdentifierAssignmentResult,
                    PaymentInstructionRequest = paymentInstructionRequestWrapper
                });
            if (paymentValidationFacadeResult.IsSucceed) return false;
            handlePaymentValidationFailure =
                FacadeFailureResponseHandler.HandlePaymentValidationFailure(paymentValidationFacadeResult,
                    paymentInstructionRequestWrapper.Content, trackingIdentifierAssignmentResult, this);
            return true;
        }

        private bool CallTraceTrackingService(PaymentInstructionRequestWrapper paymentInstructionRequestWrapper,
            out List<string> generatedTrackindIds, out PaymentInstructionFacadeOut<PaymentInstructionResponse> handleTraceTrackingFailure)
        {
            handleTraceTrackingFailure = null;
            var traceTrackingFacadeResult = _traceTrackingFacade.Call(new PaymentInstructionFacadeIn<TrackingIdentifierResult>
            {
                Data = new TrackingIdentifierResult(), //TODO: Update tracking identifer to include correct tracking id.
                PaymentInstructionRequest = paymentInstructionRequestWrapper
            });
            if (!traceTrackingFacadeResult.IsSucceed ||
                traceTrackingFacadeResult.Response.TrackingNumbers.Count !=
                paymentInstructionRequestWrapper.Content.ChequeCount + 2)
            {
                handleTraceTrackingFailure = FacadeFailureResponseHandler.HandleTraceTrackingFailure(traceTrackingFacadeResult,
                    paymentInstructionRequestWrapper.Content, new TrackingIdentifierResult());
                generatedTrackindIds = null;
                return true;
            }

            generatedTrackindIds = traceTrackingFacadeResult.Response.TrackingNumbers.Select(e => e.TrackingNumber).ToList();
            return false;
        }
    }
}
