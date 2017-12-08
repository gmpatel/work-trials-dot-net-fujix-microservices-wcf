using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Service.DTO.DipsPayload;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;
using ChequeResponse = FXA.DPSE.Service.DTO.PaymentInstruction.ChequeResponse;
using TransactionResponse = FXA.DPSE.Service.DTO.PaymentValidation.TransactionResponse;

namespace FXA.DPSE.Service.PaymentInstruction
{
    public class FacadeFailureResponseHandler
    {
        public static PaymentInstructionFacadeOut<PaymentInstructionResponse> HandleShadowPostFailure(
            PaymentInstructionFacadeOut<ShadowPostResponse> shadowPostFacadeOut,
            PaymentInstructionRequest request,
            TrackingIdentifierResult trackingIdentifier)
        {
            var paymentInstructionResponse = ResponseHelper.GetBasicPaymentInstructionResponse(request);
            paymentInstructionResponse.Response.ResultStatus = "Fail";
            paymentInstructionResponse.Response.TrackingId = trackingIdentifier.ForCredit;
            paymentInstructionResponse.FacadeErrorType = shadowPostFacadeOut.FacadeErrorType;

            if (shadowPostFacadeOut.FacadeErrorType != FacadeErrorType.ServiceFailure) return paymentInstructionResponse;

            if (shadowPostFacadeOut.Response.ChequeResponses != null && shadowPostFacadeOut.Response.ChequeResponses.Any())
            {
                if(paymentInstructionResponse.Response.ChequeResponses == null)
                    paymentInstructionResponse.Response.ChequeResponses = new List<ChequeResponse>();

                foreach (var chequeResponse in shadowPostFacadeOut.Response.ChequeResponses)
                {
                    paymentInstructionResponse.Response.ChequeResponses.Add(new DTO.PaymentInstruction.ChequeResponse()
                    {
                        ChequeResponseCode = chequeResponse.Code,
                        SequenceId = chequeResponse.SequenceId,
                        ChequeResponseDescription = chequeResponse.Description
                    });
                }
            }

            if (paymentInstructionResponse.Response.TransactionResponses == null)
                paymentInstructionResponse.Response.TransactionResponses = new List<DTO.PaymentInstruction.TransactionResponse>();

            if (shadowPostFacadeOut.Response.TransactionResponses != null && shadowPostFacadeOut.Response.TransactionResponses.Any())
            {
                foreach (var transactionResponse in shadowPostFacadeOut.Response.TransactionResponses)
                {
                    paymentInstructionResponse.Response.TransactionResponses.Add(new DTO.PaymentInstruction.TransactionResponse()
                    {
                        TransactionResponseCode = transactionResponse.Code,
                        TransactionResponseDescription = transactionResponse.Description
                    });
                }
            }

            paymentInstructionResponse.Response.TransactionResponses.Add(new DTO.PaymentInstruction.TransactionResponse()
            {
                TransactionResponseCode = shadowPostFacadeOut.Response.Code,
                TransactionResponseDescription = shadowPostFacadeOut.Response.Message
            });

            return paymentInstructionResponse;
        }

        public static PaymentInstructionFacadeOut<PaymentInstructionResponse> HandleDipsPayloadFailure(
            PaymentInstructionFacadeOut<DipsPayloadBatchResponse> dipsPayloadFacadeOut,
            PaymentInstructionRequest request,
            TrackingIdentifierResult trackingIdentifier)
        {
            var paymentInstructionResponse = ResponseHelper.GetBasicPaymentInstructionResponse(request);
            paymentInstructionResponse.Response.ResultStatus = "Fail";
            paymentInstructionResponse.Response.TrackingId = trackingIdentifier.ForCredit;
            paymentInstructionResponse.FacadeErrorType = dipsPayloadFacadeOut.FacadeErrorType;

            if (dipsPayloadFacadeOut.FacadeErrorType != FacadeErrorType.ServiceFailure) return paymentInstructionResponse;

            //TODO: Changing dips payload response ?
            //paymentInstructionResponse.ChequeResponses.Add(new DTO.PaymentInstruction.ChequeResponse()
            //{
            //});

            if (paymentInstructionResponse.Response.TransactionResponses == null)
                paymentInstructionResponse.Response.TransactionResponses = new List<DTO.PaymentInstruction.TransactionResponse>();

            paymentInstructionResponse.Response.TransactionResponses.Add(new DTO.PaymentInstruction.TransactionResponse()
            {
                TransactionResponseCode = dipsPayloadFacadeOut.Response.Code,
                TransactionResponseDescription = dipsPayloadFacadeOut.Response.Message
            });

            return paymentInstructionResponse;
        }

        public static PaymentInstructionFacadeOut<PaymentInstructionResponse> HandlePaymentValidationFailure(
            PaymentInstructionFacadeOut<PaymentValidationResponse> paymentValidationFacadeOut,
            PaymentInstructionRequest request,
            TrackingIdentifierResult trackingIdentifier, PaymentInstructionWorkflow paymentInstructionMediator)
        {
            var paymentInstructionResponse = ResponseHelper.GetBasicPaymentInstructionResponse(request);
            paymentInstructionResponse.Response.ResultStatus = "Fail";
            paymentInstructionResponse.Response.TrackingId = trackingIdentifier.ForCredit;
            paymentInstructionResponse.FacadeErrorType = paymentValidationFacadeOut.FacadeErrorType;
            if (paymentValidationFacadeOut.FacadeErrorType != FacadeErrorType.ServiceFailure) 
                return paymentInstructionResponse;

            if (paymentValidationFacadeOut.Response.Cheques != null && paymentValidationFacadeOut.Response.Cheques.Any())
            {
                foreach (var chequeResponse in paymentValidationFacadeOut.Response.Cheques)
                {
                    if (paymentInstructionResponse.Response.ChequeResponses == null)
                        paymentInstructionResponse.Response.ChequeResponses = new List<ChequeResponse>();

                    paymentInstructionResponse.Response.ChequeResponses.Add(new DTO.PaymentInstruction.ChequeResponse()
                    {
                        ChequeResponseCode = chequeResponse.Code,
                        SequenceId = chequeResponse.SequenceId,
                        ChequeResponseDescription = chequeResponse.Description
                    });
                }    
            }

            if (paymentValidationFacadeOut.Response.TransactionResponses != null && paymentValidationFacadeOut.Response.TransactionResponses.Any())
            {
                if (paymentInstructionResponse.Response.TransactionResponses == null)
                    paymentInstructionResponse.Response.TransactionResponses = new List<DTO.PaymentInstruction.TransactionResponse>();

                foreach (var transactionResponse in paymentValidationFacadeOut.Response.TransactionResponses)
                {
                    paymentInstructionResponse.Response.TransactionResponses.Add(new DTO.PaymentInstruction.TransactionResponse()
                    {
                        TransactionResponseCode = transactionResponse.Code,
                        TransactionResponseDescription = transactionResponse.Description
                    });
                }    
            }
            
            return paymentInstructionResponse;
        }

        public static PaymentInstructionFacadeOut<PaymentInstructionResponse> HandleTraceTrackingFailure(
            PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse> traceTrackingFacadeOut,
            PaymentInstructionRequest request,
            TrackingIdentifierResult trackingIdentifier)
        {
            var paymentInstructionResponse = ResponseHelper.GetBasicPaymentInstructionResponse(request);
            paymentInstructionResponse.Response.ResultStatus = "Fail";
            paymentInstructionResponse.FacadeErrorType = traceTrackingFacadeOut.FacadeErrorType;

            //paymentInstructionResponse.TrackingId = trackingIdentifier.ForCredit;
            if (traceTrackingFacadeOut.FacadeErrorType != FacadeErrorType.ServiceFailure) return paymentInstructionResponse;

            if (paymentInstructionResponse.Response.TransactionResponses == null)
                paymentInstructionResponse.Response.TransactionResponses = new List<DTO.PaymentInstruction.TransactionResponse>();

            //response.ChequeResponses
            paymentInstructionResponse.Response.TransactionResponses.Add(new DTO.PaymentInstruction.TransactionResponse()
            {
                TransactionResponseCode = traceTrackingFacadeOut.Response.Code,
                TransactionResponseDescription = traceTrackingFacadeOut.Response.Message
            });

            return paymentInstructionResponse;
        }
    }
}