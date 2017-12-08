using System;
using System.Linq;
using System.Net;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.LimitChecking;
using FXA.DPSE.Service.LimitChecking.Business;
using FXA.DPSE.Service.LimitChecking.Business.BusinessEntity;
using FXA.DPSE.Service.LimitChecking.Common;

namespace FXA.DPSE.Service.LimitChecking
{
    [LoggingBehavior]
    [ValidationBehavior("DPSE-4001")] 
    [ErrorBehavior("DPSE-4004")]
    public class LimitCheckingService : DpseServiceBase, ILimitCheckingService
    {
        private readonly IValidateTransactionLimitBusiness _transactionLimitValidator;
        private readonly ILoggingProxy _proxy; 

        public LimitCheckingService(IValidateTransactionLimitBusiness transactionLimitValidator, ILoggingProxy proxy)
        {
            _transactionLimitValidator = transactionLimitValidator;
            _proxy = proxy;
        }

        public TransactionLimitResponse Limitchecking(TransactionLimitRequest transactionLimitRequest)
        {
            try
            {
                if (transactionLimitRequest.Cheques == null || !transactionLimitRequest.Cheques.Any())
                {

                    var loggingResult = _proxy.LogEventAsync(transactionLimitRequest.TrackingId, "InputValidationError",
                        "No cheque found in the request",
                        LogLevel.Error.ToString(), string.Empty, string.Empty,
                        transactionLimitRequest.ChannelType, transactionLimitRequest.SessionId, "LimitChecking",
                        "CheckTransactionLimit");

                    if (loggingResult.HasException)
                    {
                        //TODO: What to do when logging is failed here ?    

                        return DpseResponse(new TransactionLimitResponse(transactionLimitRequest.TrackingId,
                            StatusCode.InternalProcessingError, "Logging is failed."),
                            HttpStatusCode.InternalServerError);
                    }

                    return DpseResponse(new TransactionLimitResponse 
                    {
                        TrackingId = transactionLimitRequest.TrackingId,
                        Code = StatusCode.InputValidationError,
                        Message = "No cheque found in the request."
                    }, HttpStatusCode.BadRequest);
                    
                }

                var businessResult = _transactionLimitValidator.ValidatePayloadTransactionLimit(new ChequePayload
                    {
                        TrackingId = transactionLimitRequest.TrackingId,
                        ChannelType = transactionLimitRequest.ChannelType,
                        Cheques = transactionLimitRequest.Cheques.Select(cheque => new DepositCheque(cheque.TrackingId, cheque.SequenceId, cheque.ChequeAmount, cheque.Codeline)).ToArray()                    });

                var response = new TransactionLimitResponse(transactionLimitRequest.TrackingId);

                var cheques = businessResult.Cheques.Select(depositCheque => new ChequeResponse
                {
                    SequenceId = depositCheque.SequenceId,
                    ChequeResponseCode = depositCheque.ErrorCode,
                    ChequeResponseDescription = depositCheque.Description
                }).ToList();
                cheques = cheques.OrderBy(chequeResponse => chequeResponse.SequenceId).ToList();
                response.Cheques = cheques.ToArray();

                if (businessResult.HasException)
                {
                    if (businessResult.BusinessException.ExceptionType != DpseBusinessExceptionType.BusinessRule)
                    {
                        response.Code = StatusCode.InternalProcessingError;
                        response.Message = businessResult.BusinessException.Message;
                        return DpseResponse(response, HttpStatusCode.InternalServerError);
                    }

                    response.Code = businessResult.BusinessException.ErrorCode;
                    response.Message = businessResult.BusinessException.Message;
                    return DpseResponse(response, HttpStatusCode.BadRequest);
                }
                response.Code = StatusCode.LimitCheckSuccessful;
                response.Message = "Limit check successful";
                return DpseResponse(response, HttpStatusCode.OK);
                
            }
            catch (Exception exception)
            {
                
                var loggingResult = _proxy.LogEventAsync(transactionLimitRequest.TrackingId, "InternalProcessingError", exception.Message,
                     LogLevel.Error.ToString(), exception.StackTrace, string.Empty,
                    transactionLimitRequest.ChannelType, transactionLimitRequest.SessionId, "LimitChecking",
                    "CheckTransactionLimit");

                if (loggingResult.HasException)
                {
                    //TODO: What to do when logging is failed here ?
                }

                return DpseResponse(new TransactionLimitResponse(transactionLimitRequest.TrackingId,
                    StatusCode.InternalProcessingError, "An error ocurred processing the request"),
                    HttpStatusCode.InternalServerError);
            }
        }
    }
}