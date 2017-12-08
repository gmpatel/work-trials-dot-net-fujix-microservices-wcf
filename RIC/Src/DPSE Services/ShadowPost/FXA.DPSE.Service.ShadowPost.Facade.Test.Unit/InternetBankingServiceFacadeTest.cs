using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.NabInternetBanking;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.ShadowPost.Common.Configuration;
using FXA.DPSE.Service.ShadowPost.Facade.Core;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.ShadowPost.Facade.Test.Unit
{
    public class InternetBankingServiceFacadeTest
    {
        [Fact]
        public void ShouldAuditBeforeAndAfterIbCallWithAnyResult()
        {
            var loggingProxyMock = new Mock<ILoggingProxy>();
            var configurationMock = new Mock<IShadowPostServiceConfiguration>();
            configurationMock.Setup(e => e.InternetBanking).Returns(new InternetBanking());
            
            var auditProxyMock = new Mock<IAuditProxy>();
            auditProxyMock.Setup(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostRequestSent" && request.TrackingId == "100"))).Returns(new BusinessResult()).Verifiable();
            auditProxyMock.Setup(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostRequestSent" && request.TrackingId == "200"))).Returns(new BusinessResult()).Verifiable();
            auditProxyMock.Setup(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostFailed" && request.TrackingId == "200"))).Returns(new BusinessResult()).Verifiable();
            auditProxyMock.Setup(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostSucceeded" && request.TrackingId == "100"))).Returns(new BusinessResult()).Verifiable();

            var httpClientProxyMock = new Mock<IHttpClientProxy>();

            httpClientProxyMock.Setup(e => e.PostSyncAsJson<UpdateAccountBalanceRequest, UpdateAccountBalanceResponse>(
                It.IsAny<string>(),
                It.Is<UpdateAccountBalanceRequest>(req => req.TrackingId == "100"),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<int?>()))
                .Returns(new HttpResult<UpdateAccountBalanceResponse>()
                {
                    Content = new UpdateAccountBalanceResponse(),
                    StatusCode = HttpStatusCode.OK
                });

            httpClientProxyMock.Setup(e => e.PostSyncAsJson<UpdateAccountBalanceRequest, UpdateAccountBalanceResponse>(
                It.IsAny<string>(), 
                It.Is<UpdateAccountBalanceRequest>(req=>req.TrackingId == "200"), 
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<int?>()))
                .Returns(new HttpResult<UpdateAccountBalanceResponse>()
                {
                    Content = new UpdateAccountBalanceResponse(),
                    StatusCode = HttpStatusCode.BadRequest
                });
            

            new InternetBankingServiceFacade(httpClientProxyMock.Object, loggingProxyMock.Object, auditProxyMock.Object, configurationMock.Object)
                .UpdateAccountBalance(new ShadowPostRequestWrapper()
                {
                    Header = new Dictionary<string, string>(),
                    ShadowPostRequest = new ShadowPostRequest()
                    {
                        ClientSession = new ClientSession(),
                        Cheques = new List<Cheque>()
                        {
                            new Cheque() {TrackingId = "100"},
                            new Cheque() {TrackingId = "200"}
                        },
                        DepositeAccountDetails = new DepositeAccountDetails()
                        {
                            AccountNames = new List<AccountNameDetails>()
                        }
                    }
                });

            auditProxyMock.Verify(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostRequestSent" && request.TrackingId == "100")), Times.Once);
            auditProxyMock.Verify(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostRequestSent" && request.TrackingId == "200")), Times.Once);
            auditProxyMock.Verify(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostFailed" && request.TrackingId == "200")), Times.Once);
            auditProxyMock.Verify(e => e.AuditAsync(It.Is<AuditProxyRequest>(request => request.Name == "ShadowPostSucceeded" && request.TrackingId == "100")), Times.Once);
        }
    }
}
