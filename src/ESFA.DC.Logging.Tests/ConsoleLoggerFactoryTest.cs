using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
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
            var config = new LoggerConfigurationBuilder().Build(new Mock<IApplicationLoggerSettings>().Object);

            var result = ConsoleLoggerFactory.CreateLogger(config);
            Assert.NotNull(result);
        }
    }
}
