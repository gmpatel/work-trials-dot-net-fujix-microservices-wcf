using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.NabInternetBanking;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.ShadowPost.Common.Configuration;
using FXA.DPSE.Service.ShadowPost.Facade.Core;

namespace FXA.DPSE.Service.ShadowPost.Facade
{
    public class InternetBankingServiceFacade : IInternetBankingServiceFacade
    {
        private readonly IHttpClientProxy _httpClientProxy;
        private readonly ILoggingProxy _loggingProxy;
        private readonly IAuditProxy _auditProxy;
        private readonly IShadowPostServiceConfiguration _serviceConfiguration;

        public InternetBankingServiceFacade(IHttpClientProxy httpClientProxy
            , ILoggingProxy loggingProxy
            , IAuditProxy auditProxy
            , IShadowPostServiceConfiguration serviceConfiguration)
        {
            _httpClientProxy = httpClientProxy;
            _loggingProxy = loggingProxy;
            _auditProxy = auditProxy;
            _serviceConfiguration = serviceConfiguration;
        }

        public ShadowPostFacadeResponse UpdateAccountBalance(ShadowPostRequestWrapper shadowPostRequestWrapper)
        {
            var updateBalanceRequest = CreateUpdateBalanceRequests(shadowPostRequestWrapper.ShadowPostRequest);
            var shadowPostFacadeResponse = new ShadowPostFacadeResponse();

            try
            {
                foreach (var cheque in updateBalanceRequest)
                {
                    var updateBalanceRequestAuditResult = _auditProxy.AuditAsync(new AuditProxyRequest
                    {
                        TrackingId = cheque.TrackingId,
                        Name = "ShadowPostRequestSent",
                        ChannelType = shadowPostRequestWrapper.ShadowPostRequest.ChannelType,
                        Description = string.Format("TrackingId:{0}, Codeline:{1}, Amount:{2}, ToAccount:{3}", cheque.TrackingId, cheque.CodeLine, cheque.Amount, cheque.ToAccountNumber),
                        MachineName = Environment.MachineName,
                        OperationName = "ShadowPost",
                        ServiceName = "ShadowPost",
                        SessionId = shadowPostRequestWrapper.ShadowPostRequest.ClientSession.SessionId,
                        Resource = string.Empty
                    });
                    if (updateBalanceRequestAuditResult.HasException)
                    {
                        //?
                    }

                    HttpResult<UpdateAccountBalanceResponse> updateAccountBalanceResponse;

                    if (_serviceConfiguration.InternetBanking.Enabled)
                    {
                        updateAccountBalanceResponse = _httpClientProxy
                            .PostSyncAsJson<UpdateAccountBalanceRequest, UpdateAccountBalanceResponse>(
                                _serviceConfiguration.InternetBanking.Url,
                                cheque, shadowPostRequestWrapper.Header,
                                _serviceConfiguration.InternetBanking.TimeOutSeconds);

                    }
                    else
                    {
                        updateAccountBalanceResponse = new HttpResult<UpdateAccountBalanceResponse>
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new UpdateAccountBalanceResponse
                            {
                                TrackingId = cheque.TrackingId,
                                CodeLine = cheque.CodeLine,
                                DailyLimit = "500000",
                                SettlementDate = DateTime.UtcNow.Date.ToString("yyyy-MM-dd"),
                            }
                        };
                    }

                    
                    var processedChequeResponse = new ProcessedChequeResponse
                    {
                        UpdateAccountBalanceResponse = updateAccountBalanceResponse.Content,
                        Failed = !updateAccountBalanceResponse.Succeeded
                    };
                    shadowPostFacadeResponse.ProcessedChequeResponses.Add(processedChequeResponse);

                    if (updateAccountBalanceResponse.StatusCode == HttpStatusCode.InternalServerError ||
                        updateAccountBalanceResponse.StatusCode == HttpStatusCode.Forbidden ||
                        updateAccountBalanceResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        shadowPostFacadeResponse.IbInternalProcessingFailure = true;
                        break;
                    }

                    var auditDescription = string.Format("StatusCode:{0}, Message:{1} SettlementDate:{2}",
                        updateAccountBalanceResponse.Content.Status, updateAccountBalanceResponse.Content.Message,
                        updateAccountBalanceResponse.Content.SettlementDate);

                    var updateBalanceResponseAuditResult = _auditProxy.AuditAsync(new AuditProxyRequest
                    {
                        TrackingId = cheque.TrackingId,
                        Name = string.Format("ShadowPost{0}",(updateAccountBalanceResponse.Succeeded) ? "Succeeded" : "Failed"),
                        ChannelType = shadowPostRequestWrapper.ShadowPostRequest.ChannelType,
                        Description =  auditDescription,
                        MachineName = Environment.MachineName,
                        OperationName = "ShadowPost",
                        ServiceName = "ShadowPost",
                        SessionId = shadowPostRequestWrapper.ShadowPostRequest.ClientSession.SessionId,
                        Resource = string.Empty
                    });

                    if (updateBalanceResponseAuditResult.HasException)
                    {
                        //?
                    }
                    if (!updateAccountBalanceResponse.Succeeded)
                    {
                        shadowPostFacadeResponse.IbInternalProcessingFailure = true;
                        break;
                    }
                }
                return shadowPostFacadeResponse;
            }
            catch(Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(new LoggingProxyRequest
                {
                    ChannelType = shadowPostRequestWrapper.ShadowPostRequest.ChannelType,
                    SessionId = shadowPostRequestWrapper.ShadowPostRequest.ClientSession.SessionId,
                    TrackingId = shadowPostRequestWrapper.ShadowPostRequest.TrackingId,
                    Description = exception.Message,
                    LogLevel = LogLevel.Error.ToString(),
                    LogName = "ApplicationException",
                    OperationName = "ShadowPost",
                    ServiceName = "ShadowPost",
                    Value1 = exception.StackTrace,
                    Value2 = string.Empty
                });
                if (loggingResult.HasException)
                {
                    //Do nothing !
                }
                shadowPostFacadeResponse.ProcessedChequeResponses.Clear();
                shadowPostFacadeResponse.FacadeInternalProcessingFailure = true;
                return shadowPostFacadeResponse;
            }
        }

        private List<UpdateAccountBalanceRequest> CreateUpdateBalanceRequests(ShadowPostRequest shadowPostRequest)
        {
            return shadowPostRequest.Cheques.Select(cheque => new UpdateAccountBalanceRequest
            {
                TrackingId = cheque.TrackingId,
                Amount = (cheque.ChequeAmount/100).ToString("#0.00"),
                CodeLine = cheque.Codeline, 
                TransactionNarrative = shadowPostRequest.TransactionNarrative, 
                ToAccountNumber = shadowPostRequest.DepositeAccountDetails.AccountNumber, 
                AccountHolderNames = shadowPostRequest.DepositeAccountDetails.AccountNames.Select(x => x.AccountName).ToList()
            }).ToList();
            
        }
    }
}