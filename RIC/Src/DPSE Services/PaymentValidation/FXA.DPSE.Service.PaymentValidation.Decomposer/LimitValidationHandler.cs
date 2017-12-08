using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.LimitChecking;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Common;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;
using Cheque = FXA.DPSE.Service.DTO.LimitChecking.Cheque;
using ChequeResponse = FXA.DPSE.Service.DTO.PaymentValidation.ChequeResponse;

namespace FXA.DPSE.Service.PaymentValidation.Decomposer
{
    public class LimitValidationHandler : IValidationHandler
    {
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IHttpClientProxy _httpClientProxy;

        public LimitValidationHandler(ILoggingProxy loggingProxy, IAuditProxy auditProxy, IHttpClientProxy httpClientProxy)
        {
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _httpClientProxy = httpClientProxy;
        }

        public ValidationResponse Execute(PaymentValidationRequest paymentValidationRequest, string serviceUrl)
        {
            var validationResponse = new ValidationResponse();
            try
            {
                var limitCheckingRequest = new TransactionLimitRequest
                {
                    ChannelType = paymentValidationRequest.ChannelType,
                    SessionId = paymentValidationRequest.ClientSession.SessionId,
                    TrackingId = paymentValidationRequest.TrackingId,
                    Cheques = paymentValidationRequest.Cheques.Select(cheque => new Cheque()
                    {
                        TrackingId = cheque.TrackingId,
                        ChequeAmount = cheque.ChequeAmount,
                        Codeline = cheque.Codeline,
                        SequenceId = cheque.SequenceId
                    }).ToArray()
                };
                var limitCheckingResponse = _httpClientProxy.PostSyncAsJson<TransactionLimitRequest, TransactionLimitResponse>(serviceUrl, limitCheckingRequest);
                validationResponse = BuildValidationResponse(paymentValidationRequest, limitCheckingResponse);

                if (string.IsNullOrWhiteSpace(paymentValidationRequest.TrackingId)) return validationResponse;

                var auditResult = _auditProxy.AuditAsync(
                 new AuditProxyRequest()
                 {
                     TrackingId = paymentValidationRequest.TrackingId,
                     Name = (limitCheckingResponse.Code == "DPSE-4000") ? "LimitCheckingSucceed" : "LimitCheckingFailed",
                     Description = limitCheckingResponse.Message,
                     Resource = string.Empty,
                     ChannelType = paymentValidationRequest.ChannelType,
                     MachineName = Environment.MachineName,
                     OperationName = "PaymentValidation",
                     ServiceName = "PaymentValidationService",
                     SessionId = paymentValidationRequest.ClientSession.SessionId
                 });
                
                if (!auditResult.HasException) return validationResponse;

                validationResponse.AuditError = true;
                validationResponse.PaymentValidationResponse.Cheques = null;
                validationResponse.PaymentValidationResponse.TransactionResponses = new[]
                {
                    new TransactionResponse
                    {
                        Code = limitCheckingResponse.Code, //ResponseCode.InternalProcessingError,
                        Description = limitCheckingResponse.Message //"An error occurred processing the request."
                    }
                };
                return validationResponse;
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(new LoggingProxyRequest()
                {
                    TrackingId = paymentValidationRequest.TrackingId,
                    ChannelType = paymentValidationRequest.ChannelType,
                    Description = exception.Message,
                    LogLevel = LogLevel.Error.ToString(),
                    LogName = "PaymentValidationFailedOnLimitChecking",
                    OperationName = "PaymentValidation",
                    ServiceName = "ValidationService",
                    SessionId = paymentValidationRequest.ClientSession.SessionId,
                    Value1 = exception.StackTrace,
                    Value2 = string.Empty
                });
                
                if (loggingResult.HasException)
                {
                    validationResponse.LoggingError = true;
                }
                else
                {
                    validationResponse.InternalError = true;
                }
                validationResponse.PaymentValidationResponse.Cheques = null;
                validationResponse.PaymentValidationResponse.TransactionResponses = new[]
                        {
                            new TransactionResponse
                            {
                                Code = ResponseCode.InternalProcessingError,
                                Description = "An error occurred processing the request."
                            }
                        };
                return validationResponse;
            }
        }

        private ValidationResponse BuildValidationResponse(PaymentValidationRequest validationRequest, TransactionLimitResponse response)
        {
            var paymentValidationResponse = new PaymentValidationResponse()
            {
                MessageVersion = validationRequest.MessageVersion,
                RequestUtc = validationRequest.RequestUtc,
                RequestGuid = validationRequest.RequestGuid,
                TrackingId = validationRequest.TrackingId,
                ChannelType = validationRequest.ChannelType,
                ChequeCount = validationRequest.ChequeCount,
                ClientSession = validationRequest.ClientSession
            };

            if (response.Code == "DPSE-4000")
            {
                return new ValidationResponse
                {
                    IsSucceed = true,
                    PaymentValidationResponse = paymentValidationResponse
                };
            }

            return CreateResponseFailure(response, paymentValidationResponse);
        }

        private static ValidationResponse CreateResponseFailure(TransactionLimitResponse response, PaymentValidationResponse paymentValidationResponse)
        {
            var currentChequeErrors = new List<ChequeResponse>();

            if (response.Cheques != null && response.Cheques.Any()) currentChequeErrors.AddRange(response.Cheques
                    .Where(chequeResponse => chequeResponse.ChequeResponseCode == "DPSE-4002")
                    .Select(chequeResponse => new ChequeResponse
                    {
                        SequenceId = chequeResponse.SequenceId,
                        Code = chequeResponse.ChequeResponseCode,
                        Description = chequeResponse.ChequeResponseDescription
                    }).OrderBy(e => e.SequenceId));
            
            var currentTransactionResponses = new List<TransactionResponse>
            {
                new TransactionResponse
                {
                    Code = response.Code,
                    Description = response.Message
                }
            };
            paymentValidationResponse.Cheques = currentChequeErrors.Any() ? currentChequeErrors.ToArray() : null;
            paymentValidationResponse.TransactionResponses = currentTransactionResponses.ToArray();

            return new ValidationResponse
            {
                PaymentValidationResponse = paymentValidationResponse
            };
        }
    }
}
