using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction.Facade
{
    public class PaymentValidationFacade : IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse>
    {
        private readonly IPaymentInstructionServiceConfiguration _configuration;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IHttpClientProxy _httpClientProxy;

        public PaymentValidationFacade(IPaymentInstructionServiceConfiguration configuration
            , ILoggingProxy loggingProxy
            , IAuditProxy auditProxy
            , IHttpClientProxy httpClientProxy)
        {
            _configuration = configuration;
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _httpClientProxy = httpClientProxy;
        }

        public PaymentInstructionFacadeOut<PaymentValidationResponse> Call(PaymentInstructionFacadeIn<TrackingIdentifierResult> paymentInstructionFacadeIn)
        {
            var facadeResult = new PaymentInstructionFacadeOut<PaymentValidationResponse>();
            try
            {
                var paymentValidationRequest = 
                    GetPaymentValidationRequest(paymentInstructionFacadeIn.PaymentInstructionRequest.Content,
                        paymentInstructionFacadeIn.Data);

                var paymentValidationResponse = _httpClientProxy.PostSyncAsJson<PaymentValidationRequest, PaymentValidationResponse>(
                        _configuration.PaymentValidationService.Url,
                        paymentValidationRequest, paymentInstructionFacadeIn.PaymentInstructionRequest.Header
                    );

                var isFailed = (!paymentValidationResponse.ResultStatus.Equals("Success", StringComparison.CurrentCultureIgnoreCase));
                var auditName = (isFailed) ? "PaymentValidationFailed" : "PaymentValidationSucceeded";

                //TODO: Replace tracking ID with correct value later.
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
                    Description = string.Format("Payment instruction is {0} on payment validation", (!isFailed) ? "succeed" : "failed")
                });

                if (auditResult.HasException)
                {
                    facadeResult.FacadeErrorType = FacadeErrorType.AuditFailure;
                    return facadeResult;
                }

                facadeResult.Response = paymentValidationResponse;
                if (isFailed)
                {
                    facadeResult.FacadeErrorType = FacadeErrorType.ServiceFailure;
                }
                else
                {
                    facadeResult.Response = paymentValidationResponse;
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

        private static PaymentValidationRequest GetPaymentValidationRequest(
            PaymentInstructionRequest data, TrackingIdentifierResult trackingIdentifier)
        {
            var request = new PaymentValidationRequest
            {
                MessageVersion = data.MessageVersion,
                RequestUtc = data.RequestDateTimeUtc,
                RequestGuid = data.Id,
                ValidationMode = "Default",
                TrackingId = trackingIdentifier.ForCredit,
                TotalTransactionAmount = data.TotalTransactionAmount,
                ChannelType = data.ChannelType,
                ChequeCount = data.ChequeCount,
                ClientSession = new ClientSession
                {
                    SessionId = data.ClientSession.SessionId,
                    UserId1 = data.ClientSession.UserId1,
                    UserId2 = data.ClientSession.UserId2,
                    DeviceId = data.ClientSession.DeviceId,
                    IpAddressV4 = data.ClientSession.IpAddressV4,
                    IpAddressV6 = data.ClientSession.IpAddressV6,
                    ClientName = data.ClientSession.ClientName,
                    ClientVersion = data.ClientSession.ClientVersion,
                    Os = data.ClientSession.Os,
                    OsVersion = data.ClientSession.OsVersion,
                    CaptureDevice = data.ClientSession.CaptureDevice
                },
                DepositeAccountDetails = new DepositeAccountDetails
                {
                    AccountNames = new List<AccountNameDetails>(),
                    AccountNumber = data.DepositingAccountDetails.AccountNumber,
                    AccountType = data.DepositingAccountDetails.AccountType,
                    Bsb = data.DepositingAccountDetails.BsbCode
                },
                Cheques = new List<Cheque>()
            };

            data.DepositingAccountDetails.Names
                .ToList()
                .ForEach(x => request.DepositeAccountDetails.AccountNames.Add(
                    new AccountNameDetails
                    {
                        AccountName = x.Name
                    }));
            foreach (var postingCheque in data.PostingCheques)
            {
                request.Cheques.Add(new Cheque
                {
                    ChequeAmount = postingCheque.ChequeAmount,
                    Codeline = postingCheque.Codeline,
                    SequenceId = postingCheque.SequenceId,
                    TrackingId = trackingIdentifier
                        .ForCheques
                        .First(e => e.Cheque.Codeline == postingCheque.Codeline).TrackingId
                });
            }
            return request;
        }
    }
}