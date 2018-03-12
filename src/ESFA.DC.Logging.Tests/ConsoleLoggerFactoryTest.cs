using ESFA.DC.Logging.SeriLogging;
using Moq;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class ConsoleLoggerFactoryTest
    {
        [Fact]
        public void CreateSqlServerLoggerTest()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object, "Job1");
            var config = logger.ConfigureSerilog();

            var result = ConsoleLoggerFactory.CreateLogger(config);
            Assert.NotNull(result);
        }
    }
}
