using System;
using Xunit;
using FXA.DPSE.Service.Logging.Business.BusinessEntity;
using FXA.DPSE.Service.Logging.Common;
using Moq;

namespace FXA.DPSE.Service.Logging.Business.Test.Unit
{
    public class LoggingBusinessTest
    {
        [Fact]
        public void ShouldReturnBusinessResultWithCorrectStatusCodeOnSuccessLogging()
        {
            var eventLog = CreateEventLog();
            var eventLogWriterMock = new Mock<IEventLogWriter>();
            eventLogWriterMock.Setup(e=>e.Log(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();

            var logginResult = new LoggingBusiness(eventLogWriterMock.Object).Log(eventLog);
            eventLogWriterMock.Verify(e => e.Log(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);

            Assert.False(logginResult.HasException);
        }

        [Fact]
        public void ShouldReturnBusinessExceptionWithCorrectStatusCodeOnFailLogging()
        {
             var eventLog = CreateEventLog();
            var eventLogWriterMock = new Mock<IEventLogWriter>();
            eventLogWriterMock.Setup(e=>e.Log(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object[]>())).Throws(new Exception("DymmyException"));

            var loggingResult = new LoggingBusiness(eventLogWriterMock.Object).Log(eventLog);
            eventLogWriterMock.Verify(e => e.Log(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);

            Assert.True(loggingResult.HasException);
            Assert.Equal(StatusCode.InternalProcessingError, loggingResult.BusinessException.ErrorCode);
        }

        private EventLog CreateEventLog()
        {
            var eventLog = new EventLog
            {
                ChannelType = "MCC",
                Description = "LogDescription",
                LogLevel = "Error",
                MachineName = "Machine",
                Name = "LogName",
                OperationName = "Operation",
                ServiceName = "Service",
                SessionId = "SessionId",
                TrackingId = "1100",
                Value1 = string.Empty,
                Value2 = string.Empty
            };
            return eventLog;
        }
    }
}