using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.ShadowPost.Business.Entities;
using FXA.DPSE.Service.ShadowPost.DataAccess;
using Xunit;

namespace FXA.DPSE.Service.ShadowPost.Business.Test.Unit
{
    public class ShadowPostBusinessTest
    {
        [Fact]
        public void ShouldAuditWhenReceivedDuplicateRequest()
        {
            var dataAccessMock = new Mock<IShadowPostDataAccess>();
            dataAccessMock.Setup(e => e.FindByRequestId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var auditMock = new Mock<IAuditProxy>();
            auditMock.Setup(e=>e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            new ShadowPostBusiness(new LoggingProxyFake(), auditMock.Object, dataAccessMock.Object).CheckRequestDuplication(new PayloadInfo());

            auditMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.Once());
        }

        [Fact]
        public void ShouldAssignCurrentDateWhenReceivedNulOrEmptyOrInvalidProcessingDate()
        {
            var shadowPostCheques = new List<ShadowPostedChequeInfo>
            {
                new ShadowPostedChequeInfo() {SequenceId = 1, SettlementDate = DateTime.Now.AddDays(1).Date.ToString(CultureInfo.InvariantCulture)},
                new ShadowPostedChequeInfo() {SequenceId = 2, SettlementDate = "InvalidDate"},
                new ShadowPostedChequeInfo() {SequenceId = 3, SettlementDate = string.Empty},
                new ShadowPostedChequeInfo() {SequenceId = 4, SettlementDate = "1000/30/27"},
                new ShadowPostedChequeInfo() {SequenceId = 5, SettlementDate = "12/12/2015"},
                new ShadowPostedChequeInfo() {SequenceId = 6, SettlementDate = "2015/12/24"}
            };
            var dataAccessMock = new Mock<IShadowPostDataAccess>();
            var loggingMock = new Mock<ILoggingProxy>();
            var auditMock = new Mock<IAuditProxy>();
            var result = new ShadowPostBusiness(loggingMock.Object, auditMock.Object, dataAccessMock.Object)
                .ProcessChequeProcessingDate(new PayloadInfo(), shadowPostCheques);

            Assert.Equal(DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd"), result.ShadowPostedCheques[0].SettlementDate);
            Assert.Equal(DateTime.Now.Date.ToString("yyyy-MM-dd"), result.ShadowPostedCheques[1].SettlementDate);
            Assert.Equal(DateTime.Now.Date.ToString("yyyy-MM-dd"), result.ShadowPostedCheques[2].SettlementDate);
            Assert.Equal(DateTime.Now.Date.ToString("yyyy-MM-dd"), result.ShadowPostedCheques[3].SettlementDate);
            Assert.Equal("2015-12-12", result.ShadowPostedCheques[4].SettlementDate);
            Assert.Equal("2015-12-24", result.ShadowPostedCheques[5].SettlementDate);
        }
    }
}
