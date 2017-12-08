using System;
using System.Linq;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction.Facade
{
    public class TraceTrackingFacade : IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse>
    {
        private readonly IPaymentInstructionServiceConfiguration _configuration;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IHttpClientProxy _httpClientProxy;

        public TraceTrackingFacade(IPaymentInstructionServiceConfiguration configuration
            , ILoggingProxy loggingProxy
            , IAuditProxy auditProxy
            , IHttpClientProxy httpClientProxy)
        {
            _configuration = configuration;
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _httpClientProxy = httpClientProxy;
        }

        public PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            var facadeResult = new PaymentInstructionFacadeOut<ElectronicTraceTrackingResponse>();
            try
            {
                var request = CreateTraceTrackingRequest(paymentInstructionFacadeIn);

                var traceTrackingResponse = _httpClientProxy.PostSyncAsJson<ElectronicTraceTrackingRequest, ElectronicTraceTrackingResponse>(
                    _configuration.TraceTrackingService.Url, request);

                //TODO: Replace tracking ID with correct value later.
                // var isFailed = (traceTrackingResponse.Code != "DPSE-3000");
                // var auditName = (isFailed) ? "ElectronicTraceTrackingFailed" : "ElectronicTraceTrackingSucceeded";
                // var auditResult = _auditProxy.AuditAsync(new AuditProxyRequest
                // {
                //     ChannelType = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ChannelType,
                //     TrackingId = paymentInstructionFacadeIn.Data.ForCredit,
                //     SessionId = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.SessionId,
                //     MachineName = Environment.MachineName,
                //     ServiceName = "PaymentValidation",
                //     OperationName = "PaymentValidation",
                //     Name = auditName,
                //     Resource = string.Empty,
                //     Description = string.Format("Payment instruction is {0} on trace tracking", (!isFailed) ? "succeed" : "failed")
                // });

                //if (auditResult.HasException)
                //{
                //    facadeResult.FacadeErrorType = FacadeErrorType.AuditFailure;
                //    return facadeResult;
                //}

                facadeResult.Response = traceTrackingResponse;
                if (traceTrackingResponse.Code != "DPSE-3000")
                {
                    facadeResult.FacadeErrorType = FacadeErrorType.ServiceFailure;
                }
                else
                {
                    facadeResult.IsSucceed = true;
                }
                return facadeResult;
            }
            catch (Exception exception)
            {
                //TODO: Replace tracking ID with correct value later.
                var loggingResult = _loggingProxy.LogEventAsync(
                   new LoggingProxyRequest
                   {
                       TrackingId = paymentInstructionFacadeIn.Data.ForCredit,
                       ChannelType = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ChannelType,
                       LogLevel = LogLevel.Error.ToString(),
                       SessionId = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.SessionId,
                       LogName = "ApplicationException",
                       Description = exception.Message,
                       OperationName = "PaymentInstruction",
                       ServiceName = "PaymentInstruction",
                       Value1 = "Payment Instruction service is failed on trace tracking",
                       Value2 = exception.StackTrace
                   });
                
                facadeResult.FacadeErrorType = loggingResult.HasException ? FacadeErrorType.LoggingFailure : FacadeErrorType.InternalFailure;
                return facadeResult;
            }
        }

        private static ElectronicTraceTrackingRequest CreateTraceTrackingRequest(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            //TODO: We need to include a seperated field to tell how many tracking ids are required. ChequeCount + 2 !?
            var request = new ElectronicTraceTrackingRequest
            {
                RequestGuid = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.Id,
                ChannelType = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ChannelType,
                ChequeCount = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ChequeCount + 2,
                TotalTransactionAmount = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.TotalTransactionAmount,
                ClientSession = new Session
                {
                    SessionId = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.SessionId,
                    DeviceId = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.DeviceId,
                    IpAddressV4 = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.IpAddressV4
                },
                DepositingAccountDetails = new AccountDetails
                {
                    AccountName =
                        (paymentInstructionFacadeIn.PaymentInstructionRequest.Content.DepositingAccountDetails.Names.FirstOrDefault() ==
                         null
                            ? string.Empty
                            : paymentInstructionFacadeIn.PaymentInstructionRequest.Content.DepositingAccountDetails.Names.First().Name),
                    AccountNumber = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.DepositingAccountDetails.AccountNumber,
                    AccountType = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.DepositingAccountDetails.AccountType,
                    BsbCode = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.DepositingAccountDetails.BsbCode
                }
            };
            return request;
        }
    }
}