using System;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.DipsPayload;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction.Facade
{
    public class DipsPayloadFacade : IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse>
    {
        private readonly IPaymentInstructionServiceConfiguration _configuration;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IHttpClientProxy _httpClientProxy;

        public DipsPayloadFacade(IPaymentInstructionServiceConfiguration configuration
            , ILoggingProxy loggingProxy
            , IAuditProxy auditProxy
            , IHttpClientProxy httpClientProxy)
        {
            _configuration = configuration;
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _httpClientProxy = httpClientProxy;
        }

        public PaymentInstructionFacadeOut<DipsPayloadBatchResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            var facadeResult = new PaymentInstructionFacadeOut<DipsPayloadBatchResponse>();

            try
            {
                var dipsPayloadRequest = new DipsPayloadBatchRequest
                {
                    ClientName = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.ClientName,
                    IpAddressV4 = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.IpAddressV4,
                    RequestDateTimeUtc = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.RequestDateTimeUtc,
                    MessageVersion = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.MessageVersion,
                    //TrackingId = paymentInstructionFacadeIn.Data.ForCredit
                };

                var dipsPayloadResponse = _httpClientProxy.PostSyncAsJson<DipsPayloadBatchRequest, DipsPayloadBatchResponse>(_configuration.DipsPayloadService.Url, dipsPayloadRequest);

                var isFailed = (dipsPayloadResponse.Code != "DPSE-6000");
                var auditName = (isFailed) ? "DipsPayloadFailed" : "DipsPayloadSucceeded";

                var auditResult = _auditProxy.AuditAsync(new AuditProxyRequest
                {
                    ChannelType = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ChannelType,
                    TrackingId = paymentInstructionFacadeIn.Data.ForCredit,
                    SessionId = paymentInstructionFacadeIn.PaymentInstructionRequest.Content.ClientSession.SessionId,
                    MachineName = Environment.MachineName,
                    ServiceName = "PaymentValidation",
                    OperationName = "PaymentValidation",
                    Name = auditName,
                    Resource = string.Empty,
                    Description = string.Format("Payment instruction is {0} on dips payload", (!isFailed) ? "succeed" : "failed")
                });

                if (auditResult.HasException)
                {
                    facadeResult.FacadeErrorType = FacadeErrorType.AuditFailure;
                    return facadeResult;
                }

                facadeResult.Response = dipsPayloadResponse;
                if (isFailed)
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
                        Value1 = "Payment instruction is failed on dips payload",
                        Value2 = exception.StackTrace
                    });

                facadeResult.FacadeErrorType = loggingResult.HasException ? FacadeErrorType.LoggingFailure : FacadeErrorType.InternalFailure;
                return facadeResult;
            }
        }
    }
}