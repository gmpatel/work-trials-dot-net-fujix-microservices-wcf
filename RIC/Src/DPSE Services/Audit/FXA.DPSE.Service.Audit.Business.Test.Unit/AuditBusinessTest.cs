using System;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.Audit.Common;
using FXA.DPSE.Service.Audit.Common.Configuration;
using FXA.DPSE.Service.Audit.Common.Configuration.Elements;
using FXA.DPSE.Service.Audit.DataAccess;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.Audit.Business.Test.Unit
{
    public class AuditBusinessTest
    {
        [Fact]
        public void AuditShouldReturnValidBusinessResultOnAuditSuccess()
        {
            var audiLog = CreateAuditLog();

            var configuration = new Mock<IAuditCustomConfig>();
            configuration.SetupGet(e => e.ResponseSettingsElement).Returns(() => new ResponseSettingsElement());

            var dataAccess = new Mock<IAuditDataAccess>();
            dataAccess.Setup(e => e.Insert(It.IsAny<DataAccess.Audit>())).Returns(100);

            var loggingProxy = new Mock<ILoggingProxy>();
            loggingProxy.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();
            
            var auditResult = new AuditBusiness(dataAccess.Object, loggingProxy.Object, configuration.Object).Audit(audiLog);

            loggingProxy.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.Never);
            Assert.Equal(false, auditResult.HasException);
        }

        /// <summary>
        /// This is not a "Fact" as Serilog does not bubble up any exception but still usefull when any exception happens in the AuditLogWriter.
        /// </summary>
        [Fact]
        public void ShouldSumbitErrorLogToLoggingServiceWhenAuditWriterFails()
        {
            var audiLog = CreateAuditLog();
            
            var configuration = new Mock<IAuditCustomConfig>();
            configuration.SetupGet(e => e.ResponseSettingsElement).Returns(() => new ResponseSettingsElement());

            var dataAccess = new Mock<IAuditDataAccess>();
            dataAccess.Setup(e => e.Insert(It.IsAny<DataAccess.Audit>())).Throws(new Exception("DummyException"));

            var loggingProxy = new Mock<ILoggingProxy>();
            loggingProxy.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var auditResult = new AuditBusiness(dataAccess.Object, loggingProxy.Object, configuration.Object).Audit(audiLog);

            loggingProxy.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.AtLeastOnce);
            Assert.Equal(StatusCode.DatabaseOrFileAccessError, auditResult.BusinessException.ErrorCode);
        }

        private static AuditLog CreateAuditLog()
        {
            var audiLog = new AuditLog
            {
                TrackingId = "RIC20151109299000151",
                ExternalCorrelationId = "60b43cb4-cc97-4991-aa41-66b5fd04690f",
                DocumentReferenceNumber = "299123456",
                AuditDateTime = "2015-11-15T21:44:28.560Z",
                Name = "LimitCheckSucceeded",
                Description = "Limit Check for voucher 'XXX-YYY' having $1234 amount",
                Resource = string.Empty,
                ChannelType = "MCC",
                MachineName = "Server-10aux",
                ServiceName = "PaymentInstructionService",
                OperationName = "LimitCheck",
                OperatorName = "DPSEPaymentInstruction_SVC_PROD",
            };

            return audiLog;
        }
    }
}
