using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.CodelineRules;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Common;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;
using Cheque = FXA.DPSE.Service.DTO.CodelineRules.Cheque;
using ChequeResponse = FXA.DPSE.Service.DTO.PaymentValidation.ChequeResponse;

namespace FXA.DPSE.Service.PaymentValidation.Decomposer
{
    public class CodelineValidationHandler : IValidationHandler
    {
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IHttpClientProxy _httpClientProxy;

        public CodelineValidationHandler(ILoggingProxy loggingProxy, IAuditProxy auditProxy, IHttpClientProxy httpClientProxy)
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
                var codelineRulesRequest = new CodelineRulesRequest
                {
                    ChannelType = paymentValidationRequest.ChannelType,
                    SessionId = paymentValidationRequest.ClientSession.SessionId,
                    TrackingId = paymentValidationRequest.TrackingId,
                    Cheques = paymentValidationRequest.Cheques.Select(cheque => new Cheque
                    {
                        TrackingId = cheque.TrackingId,
                        ChequeAmount = cheque.ChequeAmount,
                        Codeline = cheque.Codeline,
                        SequenceId = cheque.SequenceId
                    }).ToArray()
                };
                var codelineRulesResponse = _httpClientProxy.PostSyncAsJson<CodelineRulesRequest, CodelineRulesResponse>(serviceUrl, codelineRulesRequest);
                validationResponse = BuildValidationResponse(paymentValidationRequest, codelineRulesResponse);

                if (string.IsNullOrWhiteSpace(paymentValidationRequest.TrackingId)) return validationResponse;

                var auditResult = _auditProxy.AuditAsync(
                    new AuditProxyRequest()
                    {
                        TrackingId = paymentValidationRequest.TrackingId,
                        Name = "CodelineRulesSucceed",
                        Description = codelineRulesResponse.Message,
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
                        Code = ResponseCode.InternalProcessingError,
                        Description = "An error occurred processing the request."
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
                    LogName = "PaymentValidationFailedOnCodelineChecking",
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
            }
            return validationResponse;
        }

        private ValidationResponse BuildValidationResponse(PaymentValidationRequest validationRequest, CodelineRulesResponse response)
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

            if (response.Code == "DPSE-8800")
            {
                return new ValidationResponse
                {
                    IsSucceed = true,
                    PaymentValidationResponse = paymentValidationResponse
                };
            }

            return CreateFailureResponse(response, paymentValidationResponse);
        }

        private static ValidationResponse CreateFailureResponse(CodelineRulesResponse response, PaymentValidationResponse paymentValidationResponse)
        {
            var currentChequeErrors = new List<ChequeResponse>();
            currentChequeErrors.AddRange(response.Cheques.Select(chequeResponse => new ChequeResponse
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

            paymentValidationResponse.Cheques = currentChequeErrors.ToArray();
            paymentValidationResponse.TransactionResponses = currentTransactionResponses.ToArray();

            return new ValidationResponse
            {
                PaymentValidationResponse = paymentValidationResponse
            };
        }
    }
}
