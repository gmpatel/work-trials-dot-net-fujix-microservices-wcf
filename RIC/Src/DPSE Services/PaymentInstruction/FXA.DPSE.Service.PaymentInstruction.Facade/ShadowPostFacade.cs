using System;
using System.Linq;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction.Facade
{
    public class ShadowPostFacade : IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse>
    {
        private readonly IPaymentInstructionServiceConfiguration _configuration;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IHttpClientProxy _httpClientProxy;

        public ShadowPostFacade(IPaymentInstructionServiceConfiguration configuration
            , ILoggingProxy loggingProxy
            , IAuditProxy auditProxy
            , IHttpClientProxy httpClientProxy)
        {
            _configuration = configuration;
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _httpClientProxy = httpClientProxy;
        }

        public PaymentInstructionFacadeOut<ShadowPostResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            var facadeResult = new PaymentInstructionFacadeOut<ShadowPostResponse>();
            try
            {
                var shadowPostRequest = GetShadowPostRequest(paymentInstructionFacadeIn.PaymentInstructionRequest.Content, paymentInstructionFacadeIn.Data);

                var shadowPostResponse = _httpClientProxy.PostSyncAsJson<ShadowPostRequest, ShadowPostResponse>(
                        _configuration.ShadowPostService.Url,
                        shadowPostRequest, paymentInstructionFacadeIn.PaymentInstructionRequest.Header
                    );

                var isFailed = (shadowPostResponse.Code != "DPSE-7000");
                var auditName = (isFailed) ? "ShadowPostFailed" : "ShadowPostSucceeded";

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
                    Description = string.Format("Payment instruction is {0} on shadow posting", (!isFailed) ? "succeed" : "failed")
                });

                if (auditResult.HasException)
                {
                    facadeResult.FacadeErrorType = FacadeErrorType.AuditFailure;
                    return facadeResult;
                }

                facadeResult.Response = shadowPostResponse; 
                if (isFailed)
                {
                    facadeResult.FacadeErrorType = FacadeErrorType.ServiceFailure;
                }
                else
                {
                    facadeResult.Response = shadowPostResponse;
                    facadeResult.IsSucceed = true;
                }
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
                       Value1 = "Payment Instruction service is failed on payment validation",
                       Value2 = exception.StackTrace
                   });

                facadeResult.FacadeErrorType = loggingResult.HasException
                    ? FacadeErrorType.LoggingFailure
                    : FacadeErrorType.InternalFailure;
                return facadeResult;
            }

            return facadeResult;
        }

        private ShadowPostRequest GetShadowPostRequest(PaymentInstructionRequest paymentInstructionRequest, TrackingIdentifierResult trackingIdentifier)
        {
            var request = new ShadowPostRequest
            {
                RequestUtc = paymentInstructionRequest.RequestDateTimeUtc,
                TransactionNarrative = paymentInstructionRequest.TransactionNarrative,
                ChannelType = paymentInstructionRequest.ChannelType,
                ChequeCount = paymentInstructionRequest.ChequeCount,
                ClientSession = new ClientSession()
                {
                    CaptureDevice = paymentInstructionRequest.ClientSession.CaptureDevice,
                    ClientName = paymentInstructionRequest.ClientSession.ClientName,
                    ClientVersion = paymentInstructionRequest.ClientSession.ClientVersion,
                    DeviceId = paymentInstructionRequest.ClientSession.DeviceId,
                    IpAddressV4 = paymentInstructionRequest.ClientSession.IpAddressV4,
                    IpAddressV6 = paymentInstructionRequest.ClientSession.IpAddressV6,
                    Os = paymentInstructionRequest.ClientSession.Os,
                    OsVersion = paymentInstructionRequest.ClientSession.OsVersion,
                    SessionId = paymentInstructionRequest.ClientSession.SessionId,
                    UserId1 = paymentInstructionRequest.ClientSession.UserId1,
                    UserId2 = paymentInstructionRequest.ClientSession.UserId2
                },
                Cheques = paymentInstructionRequest.PostingCheques.Select(postingCheque=>new Cheque
                {
                    ChequeAmount = postingCheque.ChequeAmount,
                    Codeline = postingCheque.Codeline,
                    SequenceId = postingCheque.SequenceId,
                    TrackingId = trackingIdentifier.ForCheques.First(e => e.Cheque.Codeline == postingCheque.Codeline).TrackingId
                }).ToArray(),
                DepositeAccountDetails = new DepositeAccountDetails
                {
                    AccountNames = paymentInstructionRequest.DepositingAccountDetails.Names.Select(e=> new AccountNameDetails()
                    {
                        AccountName = e.Name
                    }).ToArray(),
                    AccountNumber = paymentInstructionRequest.DepositingAccountDetails.AccountNumber,
                    AccountType = paymentInstructionRequest.DepositingAccountDetails.AccountType,
                    Bsb = paymentInstructionRequest.DepositingAccountDetails.BsbCode
                },
                MessageVersion = paymentInstructionRequest.MessageVersion,
                RequestGuid = paymentInstructionRequest.Id,
                TotalTransactionAmount = paymentInstructionRequest.TotalTransactionAmount,
                TrackingId = trackingIdentifier.ForCredit
            };
            return request;
        }
    }
}
