﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dapper;
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
        [InlineData(LogLevel.Error, "JobId")]
        [InlineData(LogLevel.Error, "JobId", "taskkey")]
        [InlineData(LogLevel.Debug, "JobId")]
        [InlineData(LogLevel.Debug, "JobId", "taskkey")]
        [InlineData(LogLevel.Warning, "JobId")]
        [InlineData(LogLevel.Warning, "JobId", "taskkey")]
        [InlineData(LogLevel.Information, "JobId")]
        [InlineData(LogLevel.Information, "JobId", "taskkey")]
        public void TestLogs(LogLevel logLevel, string jobId, string taskKey = "")
        {
            _fixture.TruncateLogs();

            using (var logger = CreateSqlServerLogger(jobId, taskKey, logLevel))
            {
                switch (logLevel)
                {
                    case LogLevel.Verbose:
                        // TO DO
                        break;
                    case LogLevel.Debug:
                        logger.LogDebug($"test Debug");
                        break;
                    case LogLevel.Information:
                        logger.LogInfo($"test Information");
                        break;
                    case LogLevel.Warning:
                        logger.LogWarning($"test Warning");
                        break;
                    case LogLevel.Error:
                        logger.LogError($"test Error", new Exception("exception occured"));
                        break;
                    case LogLevel.Fatal:
                        // TO DO 
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }

            var logs = _fixture.Get<AppLog>().ToList();

            logs.Should().NotBeNull();
            logs.Should().HaveCount(1);

            var log = logs[0];

            log.Message.Should().Be($"test {logLevel}");
            log.Level.Should().Be(logLevel.ToString());
            log.MessageTemplate.Should().Be($"test {logLevel}");
            log.MachineName.Should().Be(Environment.MachineName);
            log.ProcessName.Should().Be(Process.GetCurrentProcess().ProcessName);
            log.ThreadId.Should().NotBeNullOrWhiteSpace();
            log.CallerName.Should().Be("TestLogs");
            log.SourceFile.Should().EndWith("SeriLoggerSqlServerTests.cs");
            log.LineNumber.Should().NotBeNullOrWhiteSpace();
            log.JobId.Should().Be(jobId);
            log.TaskKey.Should().Be(taskKey);

            if (logLevel == LogLevel.Error)
            {
                Assert.Equal("System.Exception: exception occured", log.Exception);
            }
        }

        private ILogger CreateSqlServerLogger(string jobId = "", string taskKey = "", LogLevel minimumLogLevel = LogLevel.Verbose, string connectionString = "AppLogs", string logsTableName = "Logs")
        {
            var config = new ApplicationLoggerSettings
            {
                MinimumLogLevel = minimumLogLevel,
                JobId = jobId,
                TaskKey = taskKey,
                ApplicationLoggerOutputSettingsCollection = new List<IApplicationLoggerOutputSettings>()
                {
                    new MsSqlServerApplicationLoggerOutputSettings()
                    {
                        ConnectionString = connectionString,
                        LogsTableName = logsTableName,
                        MinimumLogLevel = minimumLogLevel
                    }
                }
            };

            return new SeriLogger(config);
        }
    }
}