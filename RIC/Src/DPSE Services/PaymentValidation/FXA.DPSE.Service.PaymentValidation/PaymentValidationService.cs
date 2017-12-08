using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Common;
using FXA.DPSE.Service.PaymentValidation.Common.Configuration;
using FXA.DPSE.Service.PaymentValidation.Core;
using FXA.DPSE.Service.PaymentValidation.Decomposer;

namespace FXA.DPSE.Service.PaymentValidation
{
    [LoggingBehavior]
    [HeaderValidationBehavior]
    [ErrorBehavior("DPSE-8003")]
    [ValidationBehavior("DPSE-8002")]
    public class PaymentValidationService : DpseServiceBase, IPaymentValidationService
    {
        private readonly IAuditProxy _auditProxy;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IPaymentValidationMediator _paymentValidationMediator;

        public PaymentValidationService(IAuditProxy auditProxy
            , ILoggingProxy loggingProxy
            , IPaymentValidationMediator paymentValidationMediator)
        {
            _auditProxy = auditProxy;
            _loggingProxy = loggingProxy;
            _paymentValidationMediator = paymentValidationMediator;
        }

        public PaymentValidationResponse PaymentValidation(PaymentValidationRequest paymentValidationRequest)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(paymentValidationRequest.TrackingId))
                {
                    PaymentValidationResponse dpseResponse;
                    if (AuditPaymentValidation(paymentValidationRequest, "ValidationRequestReceived",
                        "Payment validation message is received", out dpseResponse))
                    {
                        dpseResponse.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message
                        return dpseResponse; 
                    }
                }

                var validationResponse = _paymentValidationMediator.ValidatePayload(paymentValidationRequest);
                if (!validationResponse.IsSucceed)
                {
                    if (validationResponse.AuditError || validationResponse.InternalError || validationResponse.LoggingError)
                    {
                        return DpseResponse(validationResponse.PaymentValidationResponse, HttpStatusCode.InternalServerError);
                    }
                
                    validationResponse.PaymentValidationResponse.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message
                    return DpseResponse(validationResponse.PaymentValidationResponse, HttpStatusCode.BadRequest);
                }

                if (!string.IsNullOrWhiteSpace(paymentValidationRequest.TrackingId))
                {
                    PaymentValidationResponse dpseResponse;
                    if (AuditPaymentValidation(paymentValidationRequest, "ValidationRequestProcessed",
                        string.Format("Payment validation request processing is {0}",
                            (validationResponse.IsSucceed) ? "successfully" : "failed"), out dpseResponse))
                    {
                        dpseResponse.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message
                        return dpseResponse;                        
                    }
                }

                validationResponse.PaymentValidationResponse.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message

                return DpseResponse(validationResponse.PaymentValidationResponse,
                    (validationResponse.IsSucceed) ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(paymentValidationRequest.TrackingId, "ApplicationException",
                    exception.Message, LogLevel.Error.ToString(), exception.StackTrace, string.Empty,
                    paymentValidationRequest.ChannelType, paymentValidationRequest.ClientSession.SessionId,
                    "PaymentValidation", "PaymentValidation");
                if (loggingResult.HasException)
                {
                    //?
                }
                var paymentValidationResponse = ValidationResponseHelper.GetBasicValidationResponse(paymentValidationRequest);
                paymentValidationResponse.Cheques = null;
                paymentValidationResponse.TransactionResponses = new[]
                        {
                            new TransactionResponse
                            {
                                Code = ResponseCode.InternalProcessingError,
                                Description = "An error occurred processing the request."
                            }
                        };
                paymentValidationResponse.ResultStatus = ValidationStatus.ValidationFailure;

                paymentValidationResponse.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message
                
                return DpseResponse(paymentValidationResponse, HttpStatusCode.InternalServerError);
            }
        }

        private bool AuditPaymentValidation(PaymentValidationRequest paymentValidationRequest, string name, string description, out PaymentValidationResponse dpseResponse)
        {
            var validationAuditResult = _auditProxy.AuditAsync(paymentValidationRequest.TrackingId,
                name, description, string.Empty, paymentValidationRequest.ChannelType,
                paymentValidationRequest.ClientSession.SessionId,
                Environment.MachineName, "PaymentValidation", "PaymentValidation");

            if (validationAuditResult.HasException) 
            {
                var response = ValidationResponseHelper.GetBasicValidationResponse(paymentValidationRequest);
                response.Cheques = null;
                response.TransactionResponses = new[]
                {
                    new TransactionResponse
                    {
                        Code = ResponseCode.InternalProcessingError,
                        Description = "Audit is failed."
                    }
                };
                {
                    dpseResponse = DpseResponse(response, HttpStatusCode.InternalServerError);
                    return true;
                }
            }
            dpseResponse = null;
            return false;
        }
    }
 }



