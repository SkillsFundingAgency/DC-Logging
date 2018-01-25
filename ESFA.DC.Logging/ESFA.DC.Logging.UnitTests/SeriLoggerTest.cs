using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;

using ESFA.DC.Logging.SeriLogging;
using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.UnitTests
{

    public class SeriLoggerTest
    {


        [Fact]
        public void LoggerInitialisedWithDefaultConnectionString()
        {
            var config = new Moq.Mock<ApplicationLoggerSettings>();
            Assert.NotNull(new SeriLogger(config.Object, "Job1"));
        }

        [Fact]
        public void LoggerInitialisedWithCosoleSettings()
        {
            var config = new Mock<ApplicationLoggerSettings>();
            config.Object.LoggerOutput = Enums.LogOutputDestination.Console;

            Assert.NotNull(new SeriLogger(config.Object, "Job1"));
        }

        [Theory]
        [InlineData(Enums.LogOutputDestination.Console)]
        [InlineData(Enums.LogOutputDestination.SqlServer)]
        public void LoggerErrorDoesNotThrowExceptionForJob(Enums.LogOutputDestination loggerOutputType)
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = loggerOutputType;

            Assert.NotNull(new SeriLogger(config, "Job1"));
        }

        [Theory]
        [InlineData(Enums.LogOutputDestination.Console)]
        [InlineData(Enums.LogOutputDestination.SqlServer)]
        public void LoggerErrorDoesNotThrowExceptionForContext(Enums.LogOutputDestination loggerOutputType)
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = loggerOutputType;

            Assert.NotNull(new SeriLogger(config, "Job1", "Context1"));
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

            Assert.NotNull(new SeriLogger(config, "Job1"));
        }

        [Fact]
        public void TestLoggerDisposed()
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = Enums.LogOutputDestination.SqlServer;

            using (var logger = new SeriLogger(config, "Job1"))
            {
                Assert.NotNull(logger);
            }
        }


        [Theory]
        [InlineData(Enums.LogLevel.Error,false)]
        [InlineData(Enums.LogLevel.Error,true)]
        [InlineData(Enums.LogLevel.Debug, false)]
        [InlineData(Enums.LogLevel.Debug, true)]
        [InlineData(Enums.LogLevel.Information, false)]
        [InlineData(Enums.LogLevel.Information, true)]
        [InlineData(Enums.LogLevel.Warning, false)]
        [InlineData(Enums.LogLevel.Warning, true)]
        public void TestLogs(Enums.LogLevel logLevel, bool withObject  )
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

                //check if there was an exception or not while executing the action
                var ex = Record.Exception(action);
                Assert.Null(ex);

            }
            
           
        }

        private ILogger CreateLogger(LogLevel logLevel, string jobId, string taskKey = "")
        {

            var config = new ApplicationLoggerSettings();
            config.ApplicationName = "Test App";
            config.LoggerOutput = LogOutputDestination.Console;
            config.MinimumLogLevel = logLevel;
            
            return new SeriLogger(config, jobId);
          
        }

    }
}
