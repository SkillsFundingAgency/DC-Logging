﻿using Dapper;
using ESFA.DC.Logging.IntergrationTests.Models;
using ESFA.DC.Logging.SeriLogging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ESFA.DC.Logging.IntergrationTests
{
    public class SeriLoggerTests : IClassFixture<TestBaseFixture>
    {
        readonly TestBaseFixture _fixture = null;

        public SeriLoggerTests(TestBaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Enums.LogLevel.Error,"JobId")]
        [InlineData(Enums.LogLevel.Error,"JobId","taskkey")]
        [InlineData(Enums.LogLevel.Debug, "JobId")]
        [InlineData(Enums.LogLevel.Debug, "JobId", "taskkey")]
        [InlineData(Enums.LogLevel.Warning, "JobId")]
        [InlineData(Enums.LogLevel.Warning, "JobId", "taskkey")]
        [InlineData(Enums.LogLevel.Information, "JobId")]
        [InlineData(Enums.LogLevel.Information, "JobId", "taskkey")]
        public void TestLogs(Enums.LogLevel logLevel, string jobId, string taskKey = "")
        {

            using (var logger = _fixture.CreateLogger(logLevel,jobId,taskKey))
            {
                switch (logLevel)
                {
                    case Enums.LogLevel.Error:
                        logger.LogError($"test Error", new Exception("exception occured"));
                        break;
                    case Enums.LogLevel.Debug:
                        logger.LogDebug($"test Debug");
                        break;
                    case Enums.LogLevel.Warning:
                        logger.LogWarning($"test Warning");
                        break;
                    case Enums.LogLevel.Information:
                        logger.LogInfo($"test Information");
                        break;
                    default:
                        break;
                }
                
            }

            var logs = _fixture.GetLogs();
            Assert.NotNull(logs);
            Assert.True(logs.Count == 1);

            var log = logs.FirstOrDefault();

            Assert.Equal("Test App", log.ApplicationId);
            Assert.Equal($"test {logLevel}", log.Message);
            Assert.Equal(logLevel.ToString(), log.Level);
            Assert.Equal($"test {logLevel}", log.MessageTemplate);

            Assert.Equal(Environment.MachineName, log.MachineName);
            Assert.Equal(Process.GetCurrentProcess().ProcessName, log.ProcessName);

            Assert.Equal($"test {logLevel}", log.MessageTemplate);

            Assert.Equal($"TestLogs", log.CallerName);
            Assert.Contains("SeriLoggerTests.cs", log.SourceFile);
            Assert.NotNull(log.LineNumber);

            Assert.Equal(jobId, log.JobId);
            Assert.Contains(taskKey, log.TaskKey);

            if (logLevel ==Enums.LogLevel.Error)
            {
                Assert.Equal("System.Exception: exception occured", log.Exception);
            }
        }

               

    }
}
