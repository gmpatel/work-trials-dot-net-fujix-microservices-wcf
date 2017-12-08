using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.TraceTracking.Business.BusinessEntities;
using FXA.DPSE.Service.TraceTracking.Business.Core;
using FXA.DPSE.Service.TraceTracking.Common;

namespace FXA.DPSE.Service.TraceTracking
{
    [ErrorBehavior("DPSE-3003")]
    [ValidationBehavior("DPSE-3002")]
    [ServiceBehavior]
    public class TraceTrackingService : DpseServiceBase, ITraceTrackingService
    {
        private readonly ITraceTrackingProcessor _traceTrackingProcessor;
        private readonly ILoggingProxy _loggingProxy;

        public TraceTrackingService(
            ITraceTrackingProcessor traceTrackingProcessor
            , IAuditProxy auditProxy
            , ILoggingProxy loggingProxy)
        {
            _traceTrackingProcessor = traceTrackingProcessor;
            _loggingProxy = loggingProxy;
        }

        public ElectronicTraceTrackingResponse ElectronicTraceTracking(ElectronicTraceTrackingRequest request)
        {
            try
            {
                if (request.ChequeCount == 0)
                {
                    return DpseResponse(new ElectronicTraceTrackingResponse
                    {
                        Code = TraceTrackingStatusCodes.InputValidationError,
                        Message = "ChequeCount field is invalid or empty."
                    }, HttpStatusCode.BadRequest);
                }
                if (request.TotalTransactionAmount == 0)
                {
                    return DpseResponse(new ElectronicTraceTrackingResponse
                    {
                        Code = TraceTrackingStatusCodes.InputValidationError,
                        Message = "TotalTransactionAmount field is invalid or empty."
                    }, HttpStatusCode.BadRequest);
                }

                var response = new ElectronicTraceTrackingResponse();

                var businessData = new TraceTrackingBusinessData
                {
                    RequestId = request.RequestGuid,
                    ChannelType = request.ChannelType,
                    ChequeCount = request.ChequeCount,
                    TotalTransactionAmount = request.TotalTransactionAmount,
                    ClientSession = new TraceTrackingSession
                    {
                        SessionId = request.ClientSession.SessionId,
                        DeviceId = request.ClientSession.DeviceId,
                        IpAddressV4 = request.ClientSession.IpAddressV4
                    },
                    DepositingAccountDetails = new TraceTrackingAccountDetails
                    {
                        AccountType = request.DepositingAccountDetails.AccountType,
                        AccountName = request.DepositingAccountDetails.AccountName,
                        AccountNumber = request.DepositingAccountDetails.AccountNumber,
                        BsbCode = request.DepositingAccountDetails.BsbCode
                    }
                };

                //TODO:  
                //The following workflow could be moved to TrackingNumberController object when it gets complicated.
                //Write Unit Test to verify the interaction between service and processor is happening in the right order.

                var duplicateIdentificationResult = _traceTrackingProcessor.CheckDuplicateRequest(businessData);
                if (duplicateIdentificationResult.HasException)
                {
                    if (duplicateIdentificationResult.BusinessException.ExceptionType ==
                        DpseBusinessExceptionType.AuditServiceException ||
                        duplicateIdentificationResult.BusinessException.ExceptionType ==
                        DpseBusinessExceptionType.LoggingServiceException)
                    {
                        return DpseResponse(new ElectronicTraceTrackingResponse
                        {
                            Code = TraceTrackingStatusCodes.InternalServerError,
                            Message = duplicateIdentificationResult.BusinessException.Message
                        }, HttpStatusCode.InternalServerError);
                    }

                    return DpseResponse(new ElectronicTraceTrackingResponse
                    {
                        Code = duplicateIdentificationResult.BusinessException.ErrorCode,
                        Message = duplicateIdentificationResult.BusinessException.Message
                    }, HttpStatusCode.BadRequest);
                }

                var traceTrackingResult = _traceTrackingProcessor.GenerateTraceTrackingNumber(businessData);

                if (traceTrackingResult.HasException)
                {
                    if (traceTrackingResult.BusinessException.ExceptionType ==
                        DpseBusinessExceptionType.AuditServiceException ||
                        traceTrackingResult.BusinessException.ExceptionType ==
                        DpseBusinessExceptionType.LoggingServiceException)
                    {
                        return DpseResponse(new ElectronicTraceTrackingResponse
                        {
                            Code = TraceTrackingStatusCodes.InternalServerError,
                            Message = traceTrackingResult.BusinessException.Message
                        }, HttpStatusCode.InternalServerError);
                    }
                    return DpseResponse(new ElectronicTraceTrackingResponse
                    {
                        Code = traceTrackingResult.BusinessException.ErrorCode,
                        Message = traceTrackingResult.BusinessException.Message
                    }, HttpStatusCode.BadRequest);
                }


                response.TrackingNumbers = new List<DTO.TraceTracking.TraceTracking>();
                response.Code = TraceTrackingStatusCodes.TraceTrackingIdentifierSuccessful;
                response.Message = "Electronic Trace Tracking Identifier is generated successfully";
                traceTrackingResult.TrackingNumbers.ToList().ForEach(x =>
                    response.TrackingNumbers.Add(new DTO.TraceTracking.TraceTracking
                    {
                        TrackingNumber = x.TrackingNumber
                    }
                        )
                    );
                return DpseResponse(response, HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(string.Empty, "InternalProcessingError", exception.Message,
                     LogLevel.Error.ToString(), exception.StackTrace, string.Empty,
                    request.ChannelType, request.ClientSession.SessionId, "LimitChecking",
                    "CheckTransactionLimit");

                if (loggingResult.HasException)
                {
                    //TODO: What to do when logging is failed here ?
                }

                return DpseResponse(new ElectronicTraceTrackingResponse
                {
                    Code = TraceTrackingStatusCodes.InternalServerError,
                    Message = "An error ocurred processing the request"
                }, HttpStatusCode.InternalServerError);
            }
        }
    }
}