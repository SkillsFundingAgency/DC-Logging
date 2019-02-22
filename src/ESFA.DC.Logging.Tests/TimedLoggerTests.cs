using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class TimedLoggerTests
    {
        [Fact]
        public void BuildTimerString()
        {
            var timedLogger = new TimedLogger(null, "Message");

            timedLogger.BuildTimerString(0).Should().Be("0 ms");
            timedLogger.BuildTimerString(1234).Should().Be("1234 ms");
        }

        [Fact]
        public void Dispose()
        {
            var loggerMock = new Mock<ILogger>();

            loggerMock.Setup(l => l.LogInfo(It.IsRegex(@"ms - Test"), null, -1, "CallerMemberName", "CallerFilePath", 100)).Verifiable();

            using (var timedLogger = new TimedLogger(loggerMock.Object, "Test", null, "CallerMemberName", "CallerFilePath", 100))
            {
            }

            loggerMock.Verify();
        }
    }
}
