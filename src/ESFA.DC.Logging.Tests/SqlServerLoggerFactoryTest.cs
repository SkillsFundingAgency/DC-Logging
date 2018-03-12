using System;
using ESFA.DC.Logging.SeriLogging;
using Moq;
using Serilog;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class SqlServerLoggerFactoryTest
    {
        [Fact]
        public void CreateSqlServerLoggerTest()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object, "Job1");
            var config = logger.ConfigureSerilog();

            var result = SqlServerLoggerFactory.CreateLogger(config, "test", "test");
            Assert.NotNull(result);
        }

        [Fact]
        public void LoggerInitialisedWithoutConnectionString()
        {
            Assert.Throws<ArgumentNullException>(() => SqlServerLoggerFactory.CreateLogger(It.IsAny<LoggerConfiguration>(), string.Empty, "test"));
        }
    }
}
