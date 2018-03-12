using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace ESFA.DC.Logging.IntergrationTests
{
    [ExcludeFromCodeCoverageAttribute]
    public class SeriLoggerTests : IClassFixture<TestBaseFixture>
    {
        private readonly TestBaseFixture _fixture = null;

        public SeriLoggerTests(TestBaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Enums.LogLevel.Error, "JobId")]
        [InlineData(Enums.LogLevel.Error, "JobId", "taskkey")]
        [InlineData(Enums.LogLevel.Debug, "JobId")]
        [InlineData(Enums.LogLevel.Debug, "JobId", "taskkey")]
        [InlineData(Enums.LogLevel.Warning, "JobId")]
        [InlineData(Enums.LogLevel.Warning, "JobId", "taskkey")]
        [InlineData(Enums.LogLevel.Information, "JobId")]
        [InlineData(Enums.LogLevel.Information, "JobId", "taskkey")]
        public void TestLogs(Enums.LogLevel logLevel, string jobId, string taskKey = "")
        {
            using (var logger = _fixture.CreateLogger(logLevel, jobId, taskKey))
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

            if (logLevel == Enums.LogLevel.Error)
            {
                Assert.Equal("System.Exception: exception occured", log.Exception);
            }
        }

       
        [Fact]
        public void TestJobContextLogs()
        {
            _fixture.DeleteLogs();
            using (var logger = LoggerManager.CreateDefaultLogger())
            {
                logger.StartContext("jobId1");
                logger.LogDebug($"test Debug");
            }

            var logs = _fixture.GetLogs();
            Assert.NotNull(logs);
            Assert.True(logs.Count == 1);

            var log = logs.FirstOrDefault();

            Assert.Equal("test Debug", log.Message);
            Assert.Equal("jobId1", log.JobId);
        }

        [Fact]
        public void TestJobWithKeyContextLogs()
        {
            _fixture.DeleteLogs();
            using (var logger = LoggerManager.CreateDefaultLogger())
            {
                logger.StartContext("jobId1", "taskkey1");
                logger.LogDebug($"test Debug");
            }

            var logs = _fixture.GetLogs();
            Assert.NotNull(logs);
            Assert.True(logs.Count == 1);

            var log = logs.FirstOrDefault();

            Assert.Equal("test Debug", log.Message);
            Assert.Equal("jobId1", log.JobId);
            Assert.Equal("taskkey1", log.TaskKey);
        }
    }
}