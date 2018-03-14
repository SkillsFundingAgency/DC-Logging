using System;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;
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
            var config = new LoggerConfigurationBuilder().Build(new Mock<IApplicationLoggerSettings>().Object);

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
