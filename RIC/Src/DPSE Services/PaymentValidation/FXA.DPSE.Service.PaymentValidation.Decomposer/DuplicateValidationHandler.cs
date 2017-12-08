using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.DuplicateIdentification;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Common;
using FXA.DPSE.Service.PaymentValidation.Decomposer.Core;
using Cheque = FXA.DPSE.Service.DTO.DuplicateIdentification.Cheque;
using ChequeResponse = FXA.DPSE.Service.DTO.PaymentValidation.ChequeResponse;

namespace FXA.DPSE.Service.PaymentValidation.Decomposer
{
    public class DuplicateValidationHandler : IValidationHandler
    {
        private readonly IHttpClientProxy _httpClientProxy;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;

        public DuplicateValidationHandler(ILoggingProxy loggingProxy, IAuditProxy auditProxy, IHttpClientProxy httpClientProxy)
        {
            _httpClientProxy = httpClientProxy;
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
        }

        public ValidationResponse Execute(PaymentValidationRequest paymentValidationRequest, string serviceUrl)
        {
            var validationResponse = new ValidationResponse();
            try
            {
                var duplicateIdentificationRequest = new DuplicateIdentificationRequest
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
                var duplicationIdentificationResponse =
                    _httpClientProxy.PostSyncAsJson<DuplicateIdentificationRequest, DuplicateIdentificationResponse>(serviceUrl,
                            duplicateIdentificationRequest);
                validationResponse = BuildValidationResponse(paymentValidationRequest, duplicationIdentificationResponse);

                if (string.IsNullOrWhiteSpace(paymentValidationRequest.TrackingId)) return validationResponse;

                var auditResult = _auditProxy.AuditAsync(
                   new AuditProxyRequest()
                   {
                       TrackingId = paymentValidationRequest.TrackingId,
                       Name = "DuplicateIdentificationSucceed",
                       Description = duplicationIdentificationResponse.Message,
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
                    LogName = "PaymentValidationFailedOnDuplicateChecking",
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

        private ValidationResponse BuildValidationResponse(PaymentValidationRequest validationRequest, DuplicateIdentificationResponse response)
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
            if (response.Code == "DPSE-8900")
            {
                return new ValidationResponse
                {
                    IsSucceed = true,
                    PaymentValidationResponse = paymentValidationResponse
                };
            }

            return CreateFailureResponse(response, paymentValidationResponse);
        }

        private static ValidationResponse CreateFailureResponse(DuplicateIdentificationResponse response, PaymentValidationResponse paymentValidationResponse)
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
