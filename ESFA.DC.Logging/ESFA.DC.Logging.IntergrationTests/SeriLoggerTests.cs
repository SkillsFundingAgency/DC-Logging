using Dapper;
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
        TestBaseFixture fixture = null;

        public SeriLoggerTests(TestBaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void TestLogError()
        {

            using (var logger = fixture.CreateLogger(Enums.LogLevel.Error))
            {
                logger.LogError($"test Error", new Exception("exception occured"));
            }

            var logs = fixture.GetLogs();
            Assert.Equal("System.Exception: exception occured", logs[0].Exception);

            AssertLog("Error");
        }

        [Fact]
        public void TestLogWarning()
        {

            using (var logger = fixture.CreateLogger(Enums.LogLevel.Warning ))
            {
                logger.LogWarning("test Warning");
            }


            AssertLog("Warning");
        }

        [Fact]
        public void TestLogInformation()
        {

            using (var logger = fixture.CreateLogger(Enums.LogLevel.Information))
            {
                logger.LogInfo( "test Information");
            }


            AssertLog("Information");
        }

        [Fact]
        public void TestLogDebug()
        {

            using (var logger = fixture.CreateLogger(Enums.LogLevel.Debug))
            {
                logger.LogDebug("test Debug");
            }


            AssertLog("Debug");
        }


            

        private void AssertLog(string logLevel)
        {
            var logs = fixture.GetLogs();

            Assert.NotNull(logs);
            Assert.True(logs.Count == 1);

            AppLogEntity log = logs.FirstOrDefault();

            Assert.Equal("Test App", log.ApplicationId);
            Assert.Equal($"test {logLevel}", log.Message);
            Assert.Equal(logLevel, log.Level);
            Assert.Equal($"test {logLevel}", log.MessageTemplate);
            
            Assert.Equal(Environment.MachineName,log.MachineName);
            Assert.Equal(Process.GetCurrentProcess().ProcessName, log.ProcessName);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId.ToString(), log.ThreadId);

            Assert.Equal($"TestLog{logLevel}", log.CallerName);
            Assert.Contains("SeriLoggerTests.cs", log.SourceFile);
            Assert.NotNull(log.LineNumber);



        }


        

    }
}
