using System;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.TraceTracking.Business.BusinessEntities;
using FXA.DPSE.Service.TraceTracking.Common;
using FXA.DPSE.Service.TraceTracking.Common.Configuration;
using FXA.DPSE.Service.TraceTracking.Common.Configuration.Elements;
using FXA.DPSE.Service.TraceTracking.DataAccess;
using FXA.DPSE.Service.TraceTracking.DataAccess.Core;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.TraceTracking.Business.Test.Unit
{
    public class TraceTrackingProcessorTest
    {
        [Fact]
        public void ShouldAuditDuplicateRequestIdentificationWithAnyResult()
        {
            //TODO: Audit design issue need to be resolved.
        }

        [Fact]
        public void ShouldCreateBusinessResultWithDuplicateStatusCode()
        {
            var configurationMock = new Mock<ITraceTrackingServiceConfiguration>();
            configurationMock.Setup(e=>e.TraceTrackingDuplicateEventCheck).Returns(new TraceTrackingDuplicateRequestElement(){TimeOutMiliseconds = 60});

            var dataAccessMock = new Mock<ITraceTrackingDataAccess>();
            dataAccessMock.Setup(e=>e.CheckRequestHasBeenProcessedRecently(It.IsAny<int>(), It.IsAny<ElectronicTraceTracking>(), It.IsAny<string>())).Returns(true);
            var result = new TraceTrackingProcessor(configurationMock.Object, dataAccessMock.Object, new LoggingProxyFake(), new AuditProxyFake())
                .CheckDuplicateRequest(new TraceTrackingBusinessData()
                {
                    ClientSession = new TraceTrackingSession(),
                    DepositingAccountDetails = new TraceTrackingAccountDetails(),
                });

            Assert.True(result.HasException);
            Assert.Equal(TraceTrackingStatusCodes.RequestAlreadyProcessedError, result.BusinessException.ErrorCode);
        }

        [Fact]
        public void ShouldSubmitErrorLogWhileProcessingRequestDuplication()
        {
            var loggingProxyMock = new Mock<ILoggingProxy>();
            loggingProxyMock.Setup(e=>e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var configurationMock = new Mock<ITraceTrackingServiceConfiguration>();
            configurationMock.Setup(e => e.TraceTrackingDuplicateEventCheck).Returns(new TraceTrackingDuplicateRequestElement() { TimeOutMiliseconds = 60 });

            var dataAccessMock = new Mock<ITraceTrackingDataAccess>();
            dataAccessMock.Setup(e => e.CheckRequestHasBeenProcessedRecently(It.IsAny<int>(), It.IsAny<ElectronicTraceTracking>(), It.IsAny<string>())).Throws(new Exception());
            var result = new TraceTrackingProcessor(configurationMock.Object, dataAccessMock.Object, loggingProxyMock.Object, new AuditProxyFake())
                .CheckDuplicateRequest(new TraceTrackingBusinessData()
                {
                    ClientSession = new TraceTrackingSession(),
                    DepositingAccountDetails = new TraceTrackingAccountDetails(),
                });

            Assert.True(result.HasException);
            Assert.Equal(TraceTrackingStatusCodes.InternalServerError, result.BusinessException.ErrorCode);
            loggingProxyMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.Once);
        }

        [Fact]
        public void ShouldAuditGeneratedTrackingIdentifiersWithAnyResult()
        {
            //TODO: Audit design issue need to be resolved.
        }

        [Fact]
        public void ShouldSubmitErrorLogWhileGeneratingTrackingIdentifiers()
        {
            var loggingProxyMock = new Mock<ILoggingProxy>();
            loggingProxyMock.Setup(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var configurationMock = new Mock<ITraceTrackingServiceConfiguration>();

            var dataAccessMock = new Mock<ITraceTrackingDataAccess>();
            dataAccessMock.Setup(e => e.GenerateTraceTrackingNumber(It.IsAny<ElectronicTraceTracking>(), It.IsAny<string>())).Throws(new Exception());
            var result = new TraceTrackingProcessor(configurationMock.Object, dataAccessMock.Object, loggingProxyMock.Object, new AuditProxyFake())
                .CheckDuplicateRequest(new TraceTrackingBusinessData()
                {
                    ClientSession = new TraceTrackingSession(),
                    DepositingAccountDetails = new TraceTrackingAccountDetails(),
                });

            Assert.True(result.HasException);
            Assert.Equal(TraceTrackingStatusCodes.InternalServerError, result.BusinessException.ErrorCode);
            loggingProxyMock.Verify(e => e.LogEventAsync(It.IsAny<LoggingProxyRequest>()), Times.Once);
        }
    }
}
