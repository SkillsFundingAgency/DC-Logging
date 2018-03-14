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

            var applicationLoggerSettingsMock = new Mock<IApplicationLoggerSettings>();
            var loggerFactoryMock = new Mock<ISerilogLoggerFactory>();
            var loggerMock = new Mock<Serilog.ILogger>();

            loggerFactoryMock.Setup(lf => lf.Build(applicationLoggerSettingsMock.Object)).Returns(loggerMock.Object);

            loggerMock.Setup(l => l.ForContext("CallerName", callerName, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();
            loggerMock.Setup(l => l.ForContext("SourceFile", filePath, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();
            loggerMock.Setup(l => l.ForContext("LineNumber", lineNumber, It.IsAny<bool>())).Returns(loggerMock.Object).Verifiable();

            var serilogger = new SeriLogger(applicationLoggerSettingsMock.Object, loggerFactoryMock.Object);

            serilogger.AddContext(callerName, filePath, lineNumber);

            loggerMock.Verify();
        }

        [Fact]
        public void LoggerInitialisedWithDefaultConnectionString()
        {
            var config = new Moq.Mock<ApplicationLoggerSettings>();
            Assert.NotNull(new SeriLogger(config.Object));
        }

        [Fact]
        public void LoggerInitialisedWithCosoleSettings()
        {
            var config = new Mock<ApplicationLoggerSettings>();
            config.Object.LoggerOutput = Enums.LogOutputDestination.Console;

            Assert.NotNull(new SeriLogger(config.Object));
        }

        [Theory]
        [InlineData(Enums.LogOutputDestination.Console)]
        [InlineData(Enums.LogOutputDestination.SqlServer)]
        public void LoggerErrorDoesNotThrowExceptionForJob(Enums.LogOutputDestination loggerOutputType)
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = loggerOutputType;

            Assert.NotNull(new SeriLogger(config));
        }

        [Theory]
        [InlineData(Enums.LogOutputDestination.Console)]
        [InlineData(Enums.LogOutputDestination.SqlServer)]
        public void LoggerErrorDoesNotThrowExceptionForContext(Enums.LogOutputDestination loggerOutputType)
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = loggerOutputType;

            Assert.NotNull(new SeriLogger(config));
        }

        [Theory]
        [InlineData(Enums.LogLevel.Verbose)]
        [InlineData(Enums.LogLevel.Debug)]
        [InlineData(Enums.LogLevel.Warning)]
        [InlineData(Enums.LogLevel.Error)]
        [InlineData(Enums.LogLevel.Information)]
        public void LoggerCreatedForEachLogLevel(Enums.LogLevel logLevel)
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = Enums.LogOutputDestination.SqlServer;
            config.MinimumLogLevel = logLevel;

            Assert.NotNull(new SeriLogger(config));
        }

        [Fact]
        public void TestLoggerDisposed()
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = Enums.LogOutputDestination.SqlServer;

            using (var logger = new SeriLogger(config))
            {
                Assert.NotNull(logger);
            }
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

        [Fact]
        public void TestDefaultLogger()
        {
            var logger = LoggerManager.CreateDefaultLogger();
            Assert.NotNull(logger);
            Assert.IsType<SeriLogger>(logger);
        }

        [Fact]
        public void TestConnectionStringLogger()
        {
            var logger = LoggerManager.CreateLogger("test connection string");
            Assert.NotNull(logger);
            Assert.IsType<SeriLogger>(logger);
        }

        private ILogger CreateLogger(LogLevel logLevel, string jobId, string taskKey = "")
        {
            var config = new ApplicationLoggerSettings
            {
                LoggerOutput = LogOutputDestination.Console,
                MinimumLogLevel = logLevel,
                JobId = jobId,
                TaskKey = taskKey
            };

            return new SeriLogger(config);
        }
    }
}