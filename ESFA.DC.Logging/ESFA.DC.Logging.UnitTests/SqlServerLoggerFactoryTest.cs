using ESFA.DC.Logging.SeriLogging;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ESFA.DC.Logging.UnitTests
{
    public class SqlServerLoggerFactoryTest
    {
        [Fact]
        public void CreateSqlServerLoggerTest()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object);
            var config = logger.ConfigureSerilog();

            var result = SqlServerLoggerFactory.CreateLogger(config, "test", "test");
            Assert.NotNull(result);
        }

        [Fact]
        public void LoggerInitialisedWithoutConnectionString()
        {
            Assert.Throws<ArgumentNullException>(() => SqlServerLoggerFactory.CreateLogger(It.IsAny<LoggerConfiguration>(), "", "test"));
        }
    }
}
