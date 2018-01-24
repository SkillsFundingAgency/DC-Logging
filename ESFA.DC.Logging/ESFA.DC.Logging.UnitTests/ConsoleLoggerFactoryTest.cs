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
    public class SerilogSqlServerLoggerTest
    {
        [Fact]
        public void CreateSqlServerLoggerTest()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object);
            var config = logger.ConfigureSerilog();

            var result = ConsoleLoggerFactory.CreateLogger(config);
            Assert.NotNull(result);
        }

    }
}
