using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using FXA.DPSE.Framework.Common.Security.SHA1;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.PaymentInstruction.Business;
using FXA.DPSE.Service.PaymentInstruction.Common;
using FXA.DPSE.Service.PaymentInstruction.Common.Converters;
using FXA.DPSE.Service.PaymentInstruction.Core;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction
{
    [LoggingBehavior]
    [HeaderValidationBehavior]
    [ErrorBehavior("DPSE-9003")]
    [ValidationBehavior("DPSE-9001")]
    public class PaymentInstructionService : DpseServiceBase, IPaymentInstructionService
    {
        private readonly IPaymentInstructionBusiness _paymentInstructionProcessor;
        private readonly IPaymentInstructionWorkflow _paymentInstructionMediator;
        private readonly IAuditProxy _auditProxy;
        private readonly ILoggingProxy _logginProxy;

        public PaymentInstructionService(IPaymentInstructionBusiness paymentInstructionProcessor
            , IPaymentInstructionWorkflow paymentInstructionMediator
            , IAuditProxy auditProxy
            , ILoggingProxy logginProxy)
        {
            _paymentInstructionProcessor = paymentInstructionProcessor;
            _paymentInstructionMediator = paymentInstructionMediator;
            _auditProxy = auditProxy;
            _logginProxy = logginProxy;
        }

        public PaymentInstructionResponse PaymentInstruction(PaymentInstructionRequest request)
        {
            try
            {
                //TODO: Develop custom validation attributes instead.
                ValidatePaymentInstructionBusinessData(request);

                var auditResult = _auditProxy.AuditAsync(string.Empty, "PaymentInstructionStarted",
                    "Payment instruction service received a request", string.Empty, request.ChannelType,
                    request.ClientSession.SessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");
                if (auditResult.HasException)
                {
                    //Log ?
                    var startAuditFailResponse = GetBasicResponse(request);
                    startAuditFailResponse.ResultStatus = "Fail";
                    startAuditFailResponse.TransactionResponses.Add(new TransactionResponse()
                    {
                        TransactionResponseCode = auditResult.BusinessException.ErrorCode,
                        TransactionResponseDescription = auditResult.BusinessException.Message
                    });return DpseResponse(startAuditFailResponse, HttpStatusCode.InternalServerError);
                }

                IDictionary<string, string> headers = new Dictionary<string, string>();

                if (WebOperationContext.Current != null)
                {
                    foreach (var headerKey in WebOperationContext.Current.IncomingRequest.Headers.AllKeys)
                    {
                        var key = string.Copy(headerKey);
                        var value = string.Copy(WebOperationContext.Current.IncomingRequest.Headers[key]);
                        headers.Add(key, value);
                    }    
                }
                
                var result = _paymentInstructionMediator.Run(new PaymentInstructionRequestWrapper()
                {
                    Content = request,
                    Header = headers //webHeaderCollection.AllKeys.ToDictionary(webHeaderKey => webHeaderKey, webHeaderKey => webHeaderCollection[webHeaderKey])
                });
                var auditFinalResult = _auditProxy.AuditAsync(result.Response.TrackingId, "PaymentInstructionCompleted",
                    "Payment instruction service completed processing the request", string.Empty, request.ChannelType,
                    request.ClientSession.SessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");
                if (auditFinalResult.HasException)
                {
                    //Log ?
                    var startAuditFailResponse = GetBasicResponse(request);
                    startAuditFailResponse.ResultStatus = "Fail";
                    startAuditFailResponse.TransactionResponses.Add(new TransactionResponse()
                    {
                        TransactionResponseCode = auditFinalResult.BusinessException.ErrorCode,
                        TransactionResponseDescription = auditFinalResult.BusinessException.Message
                    });
                    return DpseResponse(startAuditFailResponse, HttpStatusCode.InternalServerError);
                }
                if (!result.IsSucceed)
                {
                    result.Response.ResultStatus = "Fail";
                    if (result.BusinessErrorCode == StatusCode.EntityNotFound)
                        return DpseResponse(result.Response, HttpStatusCode.NotFound);

                    switch (result.FacadeErrorType)
                    {
                        case FacadeErrorType.AuditFailure:
                        case FacadeErrorType.InternalFailure:
                        case FacadeErrorType.LoggingFailure:
                        case FacadeErrorType.DataAccessOrFileSystemFailure:
                            return DpseResponse(result.Response, HttpStatusCode.InternalServerError);
                        case FacadeErrorType.ServiceFailure:
                        case FacadeErrorType.BusinessFailure:
                            return DpseResponse(result.Response, HttpStatusCode.BadRequest);
                    }
                }
                var response = DpseResponse(result.Response, HttpStatusCode.OK);

                response.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message

                return response;

            }
            catch (Exception exception)
            {
                var loggingResult = _logginProxy.LogEventAsync(string.Empty, "PaymentInstructionFailed", exception.Message,
                    LogLevel.Fatal.ToString(), exception.StackTrace, string.Empty, request.ChannelType,
                    request.ClientSession.SessionId,
                    "PaymentInstruction", "PaymentInstruction");
                if (loggingResult.HasException)
                {
                    //?
                }
                var errorResponse = GetBasicResponse(request);
                errorResponse.ResultStatus = "Fail";
                errorResponse.TransactionResponses.Add(new TransactionResponse()
                {
                    TransactionResponseCode = StatusCode.InternalProcessingError,
                    TransactionResponseDescription = string.Format("An error occurred processing the request. {0}. {1}.", exception.Message, exception.StackTrace)
                });

                errorResponse.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message

                return DpseResponse(errorResponse, HttpStatusCode.InternalServerError);
            }
        }

        public PaymentInstructionStatusUpdateResponse Status(PaymentInstructionStatusUpdateRequest request)
        {
            var response = new PaymentInstructionStatusUpdateResponse();
            try
            {
                var businessData = request.GetPaymentInstructionStatusUpdateBusinessData();
                var businessResult = _paymentInstructionProcessor.UpdateStatus(businessData);

                if (businessResult.HasException)
                {
                    response.Code = businessResult.BusinessException.ErrorCode;
                    response.Message = businessResult.BusinessException.Message;

                    if (businessResult.BusinessException.ErrorCode == StatusCode.EntityNotFound)
                        return DpseResponse(response, HttpStatusCode.NotFound);
                    //No business rule error in this case. i.e. 400
                    return DpseResponse(response, HttpStatusCode.InternalServerError);
                }
                response.Code = StatusCode.PaymentInstructionSuccessful;
                response.Message = "Payment instruction status update success";
                return DpseResponse(response, HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                var loggingResult = _logginProxy.LogEventAsync(string.Empty, "PaymentInstructionStatusUpdateFailed", exception.Message,
                                    LogLevel.Fatal.ToString(), exception.StackTrace, string.Empty, request.ChannelType,
                                    request.ClientSession.SessionId,
                                    "PaymentInstruction", "Status");
                if (loggingResult.HasException)
                {
                    //?
                }
                var errorResponse = new PaymentInstructionStatusUpdateResponse
                {
                    Code = StatusCode.InternalProcessingError,
                    Message = string.Format("An error occurred processing the request. {0}. {1}.", exception.Message, exception.StackTrace)
                };
                return DpseResponse(errorResponse, HttpStatusCode.InternalServerError);
            }
        }

        private static PaymentInstructionResponse GetBasicResponse(PaymentInstructionRequest paymentInstructionRequest)
        {
            var response = new PaymentInstructionResponse
            {
                TransactionResponses = new List<TransactionResponse>(),
                ChannelType = paymentInstructionRequest.ChannelType,
                ChequeCount = paymentInstructionRequest.ChequeCount,
                ClientSession = paymentInstructionRequest.ClientSession,
                MessageVersion = paymentInstructionRequest.MessageVersion,
                RequestDateTimeUtc = paymentInstructionRequest.RequestDateTimeUtc,
                RequestGuid = paymentInstructionRequest.Id,
                TotalTransactionAmount = paymentInstructionRequest.TotalTransactionAmount
            };
            return response;
        }

        private static void ValidatePaymentInstructionBusinessData(PaymentInstructionRequest data)
        {
            if (data.ChequeCount != data.PostingCheques.Count)
            {
                throw new DpseValidationException(
                    "Cheque count is not matching with the entry count of the PostingCheques.", StatusCode.InputValidationError);
            }

            for (var i = 0; i < data.PostingCheques.Count; i++)
            {
                var entry = data.PostingCheques[i];

                if (!entry.FrontImage.ValidateWithSHA1(entry.FrontImageSha))
                {
                    throw new DpseValidationException(string.Format(
                            "FrontImage Base64 string of PostingCheque[{0}] is not validated against the SHA1 of FrontImage.", i
                        ), StatusCode.InputValidationError);
                }

                if (!entry.RearImage.ValidateWithSHA1(entry.RearImageSha))
                {
                    throw new DpseValidationException(string.Format(
                        "RearImage Base64 string of PostingCheque[{0}] is not validated against the SHA1 of RearImage.",
                        i
                        ), StatusCode.InputValidationError);
                }
            }
        }
    }
}