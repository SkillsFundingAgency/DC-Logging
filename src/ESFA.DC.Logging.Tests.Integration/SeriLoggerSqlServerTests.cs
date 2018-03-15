using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dapper;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Logging.Tests.Integration.Fixtures;
using ESFA.DC.Logging.Tests.Integration.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.Logging.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public class SeriLoggerSqlServerTests : IClassFixture<SqlDatabaseFixture>
    {
        private readonly SqlDatabaseFixture _fixture;

        public SeriLoggerSqlServerTests(SqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(LogLevel.Fatal, "JobId")]
        [InlineData(LogLevel.Fatal, "JobId", "taskkey")]
        [InlineData(LogLevel.Error, "JobId")]
        [InlineData(LogLevel.Error, "JobId", "taskkey")]
        [InlineData(LogLevel.Debug, "JobId")]
        [InlineData(LogLevel.Debug, "JobId", "taskkey")]
        [InlineData(LogLevel.Warning, "JobId")]
        [InlineData(LogLevel.Warning, "JobId", "taskkey")]
        [InlineData(LogLevel.Information, "JobId")]
        [InlineData(LogLevel.Information, "JobId", "taskkey")]
        [InlineData(LogLevel.Verbose, "JobId")]
        [InlineData(LogLevel.Verbose, "JobId", "taskkey")]
        public void TestLogs(LogLevel logLevel, string jobId, string taskKey = "")
        {
            _fixture.TruncateLogs();

            using (var logger = CreateSqlServerLogger(jobId, taskKey, logLevel))
            {
                switch (logLevel)
                {
                    case LogLevel.Verbose:
                        logger.LogVerbose("Test Verbose");
                        break;
                    case LogLevel.Debug:
                        logger.LogDebug("Test Debug");
                        break;
                    case LogLevel.Information:
                        logger.LogInfo("Test Information");
                        break;
                    case LogLevel.Warning:
                        logger.LogWarning("Test Warning");
                        break;
                    case LogLevel.Error:
                        logger.LogError("Test Error", new Exception("Exception occured."));
                        break;
                    case LogLevel.Fatal:
                        logger.LogFatal("Test Fatal", new Exception("Exception occured."));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }

            var logs = _fixture.Get<AppLog>().ToList();

            logs.Should().NotBeNull();
            logs.Should().HaveCount(1);

            var log = logs[0];

            log.Message.Should().Be($"Test {logLevel}");
            log.Level.Should().Be(logLevel.ToString());
            log.MessageTemplate.Should().Be($"Test {logLevel}");
            log.MachineName.Should().Be(Environment.MachineName);
            log.ProcessName.Should().Be(Process.GetCurrentProcess().ProcessName);
            log.ThreadId.Should().NotBeNullOrWhiteSpace();
            log.CallerName.Should().Be("TestLogs");
            log.SourceFile.Should().EndWith("SeriLoggerSqlServerTests.cs");
            log.LineNumber.Should().NotBeNullOrWhiteSpace();
            log.JobId.Should().Be(jobId);
            log.TaskKey.Should().Be(taskKey);

            if (logLevel == LogLevel.Error || logLevel == LogLevel.Fatal)
            {
                Assert.Equal("System.Exception: Exception occured.", log.Exception);
            }
        }

        private ILogger CreateSqlServerLogger(string jobId = "", string taskKey = "", LogLevel minimumLogLevel = LogLevel.Verbose, string connectionString = "AppLogs", string logsTableName = "Logs")
        {
            var config = new ApplicationLoggerSettings
            {
                ApplicationLoggerOutputSettingsCollection = new List<IApplicationLoggerOutputSettings>()
                {
                    new MsSqlServerApplicationLoggerOutputSettings()
                    {
                        MinimumLogLevel = LogLevel.Verbose
                    }
                }
            };

            var executionContext = new ExecutionContext()
            {
                JobId = jobId,
                TaskKey = taskKey,
            };

            return new SeriLogger(config, executionContext, new SerilogLoggerFactory(new LoggerConfigurationBuilder()));
        }
    }
}