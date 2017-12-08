using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.ShadowPost.Business;
using FXA.DPSE.Service.ShadowPost.Business.Entities;
using FXA.DPSE.Service.ShadowPost.Common;
using FXA.DPSE.Service.ShadowPost.Core;
using FXA.DPSE.Service.ShadowPost.Facade.Core;

namespace FXA.DPSE.Service.ShadowPost
{
    [LoggingBehavior]
    [ErrorBehavior("DPSE-7003")]
    [ValidationBehavior("DPSE-7002")]
    [HeaderValidationBehavior]
    public class ShadowPostService : DpseServiceBase, IShadowPostService
    {
        private readonly IShadowPostBusiness _shadowPostBusiness;
        private readonly IInternetBankingServiceFacade _internetBankingServiceFacade;
        private readonly ILoggingProxy _loggingProxy;

        public ShadowPostService(IShadowPostBusiness shadowPostBusiness, IInternetBankingServiceFacade internetBankingServiceFacade, ILoggingProxy loggingProxy)
        {
            _shadowPostBusiness = shadowPostBusiness;
            _internetBankingServiceFacade = internetBankingServiceFacade;
            _loggingProxy = loggingProxy;
        }

        public ShadowPostResponse ShadowPost(ShadowPostRequest request)
        {
            
            try
            {
                var response = ShadowPostResponseHelper.GetBasicShadowPostResponse(request);
                ShadowPostResponse shadowPost;
                if (HandleDuplicateRequest(request, response, out shadowPost))
                {
                    shadowPost.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message
                    return shadowPost;
                }

                var shadowPostStoreResult = _shadowPostBusiness.StoreShadowPostRequest(CreatePayloadInfo(request));
                if (shadowPostStoreResult.HasException)
                {
                    response.ResultStatus = "Fail";
                    response.Code = StatusCode.InternalProcessingError;
                    response.Message = "An error occurred processing the request";

                    response.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message
                
                    return DpseResponse(response, HttpStatusCode.InternalServerError);
                }

                ShadowPostFacadeResponse internetBankingResult;
                ShadowPostResponse shadowPostResponse1;
                if (HandleFacadeResponse(request, response, out internetBankingResult, out shadowPostResponse1)) return shadowPostResponse1;

                response = HandleShadowPostedChequeProcessingDate(request, internetBankingResult, response);

                response.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message

                return response;
            }
            catch (Exception exception)
            {
                var loggingResult = _loggingProxy.LogEventAsync(new LoggingProxyRequest
                {
                    ChannelType = request.ChannelType,
                    SessionId = request.ClientSession.SessionId,
                    TrackingId = request.TrackingId,
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
                    //Do nothing !?
                }
                var response = ShadowPostResponseHelper.GetBasicShadowPostResponse(request);
                response.ResultStatus = "Fail";
                response.Code = StatusCode.InternalProcessingError;
                response.Message = "An error occurred processing the request";

                response.Code = null; // Fix: Bug 25422:Payment Validation - Serivce Respond Code shouldn't be in the service respond message

                return DpseResponse(response, HttpStatusCode.InternalServerError);
            }
        }

        private bool HandleFacadeResponse(ShadowPostRequest request, ShadowPostResponse shadowPostResponse, out ShadowPostFacadeResponse internetBankingResult, out ShadowPostResponse response)
        {
            response = null;
            var webHeaderCollection = WebOperationContext.Current.IncomingRequest.Headers;
            internetBankingResult = _internetBankingServiceFacade.UpdateAccountBalance(new ShadowPostRequestWrapper
            {
                ShadowPostRequest = request,
                Header = webHeaderCollection.AllKeys.ToDictionary(webHeaderKey => webHeaderKey, webHeaderKey => webHeaderCollection[webHeaderKey])
            });
            if (internetBankingResult.FacadeInternalProcessingFailure)
            {
                shadowPostResponse.ResultStatus = "Fail";
                shadowPostResponse.Code = StatusCode.InternalProcessingError;
                shadowPostResponse.Message = "An error occurred processing the request";
                {
                    response = DpseResponse(shadowPostResponse, HttpStatusCode.InternalServerError);
                    return true;
                }
            }
            if (internetBankingResult.IbInternalProcessingFailure)
            {
                var failedCheque = internetBankingResult.ProcessedChequeResponses.First(e => e.Failed);
                shadowPostResponse.ResultStatus = "Fail";

                string dpseStatusCode;
                var httpStatusCode = ShadowPostResponseHelper.GetHttpStatusCodeByIbResponse(failedCheque.UpdateAccountBalanceResponse, out dpseStatusCode);
                shadowPostResponse.Code = dpseStatusCode;
                shadowPostResponse.Message = failedCheque.UpdateAccountBalanceResponse.Message;

                shadowPostResponse.ChequeResponses = new List<ChequeResponse>();
                shadowPostResponse.TransactionResponses = new List<TransactionResponse>();

                shadowPostResponse.ChequeResponses.Add(new ChequeResponse
                {
                    Code = dpseStatusCode,
                    Description = failedCheque.UpdateAccountBalanceResponse.Message,
                    SequenceId = request.Cheques.First(e => e.TrackingId == failedCheque.UpdateAccountBalanceResponse.TrackingId).SequenceId
                });

                //What is the difference ?
                shadowPostResponse.TransactionResponses.Add(new TransactionResponse
                {
                    Code = dpseStatusCode,
                    Description = failedCheque.UpdateAccountBalanceResponse.Status.Message
                });

                response = DpseResponse(shadowPostResponse, httpStatusCode);

                return true;
            }
            return false;
        }

        private ShadowPostResponse HandleShadowPostedChequeProcessingDate(ShadowPostRequest request, ShadowPostFacadeResponse internetBankingResult, ShadowPostResponse shadowPostResponse)
        {
            var shadowPostedList = internetBankingResult.ProcessedChequeResponses
                .Select(processedChequeResponse => new ShadowPostedChequeInfo
            {
                SequenceId = request.Cheques.First(e => e.TrackingId == processedChequeResponse.UpdateAccountBalanceResponse.TrackingId).SequenceId,
                TrackingId = processedChequeResponse.UpdateAccountBalanceResponse.TrackingId,
                Codeline = processedChequeResponse.UpdateAccountBalanceResponse.CodeLine,
                SettlementDate = processedChequeResponse.UpdateAccountBalanceResponse.SettlementDate
            }).ToList();

            var shadowPostBusinessResult = _shadowPostBusiness.ProcessChequeProcessingDate(CreatePayloadInfo(request), shadowPostedList);
            if (shadowPostBusinessResult.HasException)
            {
                shadowPostResponse.ResultStatus = "Fail";
                shadowPostResponse.Code = StatusCode.InternalProcessingError;
                shadowPostResponse.Message = "An error occurred processing the request";
                {
                    return DpseResponse(shadowPostResponse, HttpStatusCode.InternalServerError);
                }
            }
            shadowPostResponse.ChequeResponses = new List<ChequeResponse>();
            foreach (var shadowPostChequeInfo in shadowPostBusinessResult.ShadowPostedCheques)
            {
                shadowPostResponse.ChequeResponses.Add(new ChequeResponse
                {
                    SequenceId = shadowPostChequeInfo.SequenceId,
                    TrackingId = shadowPostChequeInfo.TrackingId,
                    Code = StatusCode.ShadowPostSuccessful,
                    Description = "Shadow post is successful",
                    ProcessingDate = shadowPostChequeInfo.SettlementDate
                });
            }
            shadowPostResponse.Code = StatusCode.ShadowPostSuccessful;
            shadowPostResponse.Message = "Shadow post is successful";
            shadowPostResponse.ResultStatus = "Success";
            return DpseResponse(shadowPostResponse, HttpStatusCode.OK);
                }

        private bool HandleDuplicateRequest(ShadowPostRequest request, ShadowPostResponse shadowPostResponse, out ShadowPostResponse shadowPost)
        {
            shadowPost = null;
            var duplicateCheckResult = _shadowPostBusiness.CheckRequestDuplication(CreatePayloadInfo(request));
            if (duplicateCheckResult.HasException)
            {
                shadowPostResponse.ResultStatus = "Fail";
                shadowPostResponse.Code = StatusCode.InternalProcessingError;
                shadowPostResponse.Message = "An error occurred processing the request";
                {
                    shadowPost = DpseResponse(shadowPostResponse, HttpStatusCode.InternalServerError);
                    return true;
                }
            }
            if (duplicateCheckResult.IsDuplicated)
            {
                shadowPostResponse.ResultStatus = "Fail";
                shadowPostResponse.Code = StatusCode.DuplicateRequestError;
                shadowPostResponse.Message = "The request has been already proccessed";
                {
                    shadowPost = DpseResponse(shadowPostResponse, HttpStatusCode.BadRequest);
                    return true;
                }
            }
            return false;
            }

        private PayloadInfo CreatePayloadInfo(ShadowPostRequest request)
        {
            var businessData = new PayloadInfo
            {
                RequestUtc = request.RequestUtc,
                RequestGuid = request.RequestGuid,
                TrackingId = request.TrackingId,
                ChannelType = request.ChannelType,
                ChequeCount = request.ChequeCount,
                SessionId = request.ClientSession.SessionId,
                DeviceId = request.ClientSession.DeviceId,
                IpAddressV4 = request.ClientSession.IpAddressV4
            };
            return businessData;
        }
    }
}