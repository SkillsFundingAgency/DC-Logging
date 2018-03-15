using System;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class SeriLoggerTests
    {
        [Fact]
        public void AddContext()
        {
            var callerName = "CallerNameValue";
            var filePath = "SourceFileValue";
            var lineNumber = 1;
            var jobId = "JobId";
            var taskKey = "TaskKey";

            var applicationLoggerSettingsMock = new Mock<IApplicationLoggerSettings>();
            var loggerFactoryMock = new Mock<ISerilogLoggerFactory>();
            var loggerMock = new Mock<Serilog.ILogger>();

            loggerFactoryMock.Setup(lf => lf.Build(applicationLoggerSettingsMock.Object)).Returns(loggerMock.Object);

            loggerMock.Setup(l => l.ForContext("CallerName", callerName, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();
            loggerMock.Setup(l => l.ForContext("SourceFile", filePath, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();
            loggerMock.Setup(l => l.ForContext("LineNumber", lineNumber, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();
            loggerMock.Setup(l => l.ForContext("JobId", jobId, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();
            loggerMock.Setup(l => l.ForContext("TaskKey", taskKey, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();

            var serilogger = new SeriLogger(applicationLoggerSettingsMock.Object, null, loggerFactoryMock.Object);

            serilogger.AddContext(callerName, filePath, lineNumber, jobId, taskKey);

            loggerMock.Verify();
        }

        [Fact]
        public void Constructor()
        {
            var loggerFactoryMock = new Mock<ISerilogLoggerFactory>();
            var applicationLoggerSettingsMock = new Mock<IApplicationLoggerSettings>();

            loggerFactoryMock.Setup(lf => lf.Build(applicationLoggerSettingsMock.Object)).Verifiable();

            var serilogger = new SeriLogger(applicationLoggerSettingsMock.Object, null, loggerFactoryMock.Object);

            loggerFactoryMock.Verify();
        }

        [Theory]
        [InlineData(Enums.LogLevel.Error, false)]
        [InlineData(Enums.LogLevel.Error, true)]
        [InlineData(Enums.LogLevel.Debug, false)]
        [InlineData(Enums.LogLevel.Debug, true)]
        [InlineData(Enums.LogLevel.Information, false)]
        [InlineData(Enums.LogLevel.Information, true)]
        [InlineData(Enums.LogLevel.Warning, false)]
        [InlineData(Enums.LogLevel.Warning, true)]
        public void TestLogs(Enums.LogLevel logLevel, bool withObject)
        {
            Action action = null;

            using (var logger = CreateLogger(logLevel, "jobId", "taskKey"))
            {
                switch (logLevel)
                {
                    case Enums.LogLevel.Error:
                        action = () => logger.LogError($"test Error", new Exception("exception occured"), withObject ? new object[] { new object() } : null);
                        break;

                    case Enums.LogLevel.Debug:
                        action = () => logger.LogDebug($"test Debug", withObject ? new object[] { new object() } : null);
                        break;

                    case Enums.LogLevel.Warning:
                        action = () => logger.LogWarning($"test Warning", withObject ? new object[] { new object() } : null);
                        break;

                    case Enums.LogLevel.Information:
                        action = () => logger.LogInfo($"test Information", withObject ? new object[] { new object() } : null);
                        break;

                    default:
                        break;
                }

                var ex = Record.Exception(action);
                Assert.Null(ex);
            }
        }

        private ILogger CreateLogger(LogLevel logLevel, string jobId, string taskKey = "")
        {
            var config = new ApplicationLoggerSettings
            {
                MinimumLogLevel = logLevel,
                JobId = jobId,
                TaskKey = taskKey
            };

            var executionContext = new ExecutionContext();

            return new SeriLogger(config, executionContext);
        }
    }
}