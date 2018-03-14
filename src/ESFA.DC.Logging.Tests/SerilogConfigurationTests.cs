using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class SerilogConfigurationTests
    {
        [Fact]
        public void SetupSeriLogConfigForJob()
        {
            var config = new LoggerConfigurationBuilder().Build(new Mock<IApplicationLoggerSettings>().Object);

            Assert.NotNull(config);
        }

        [Fact]
        public void SetupSeriLogConfigForTaskKey()
        {
            var config = new LoggerConfigurationBuilder().Build(new Mock<IApplicationLoggerSettings>().Object);

            Assert.NotNull(config);
        }
    }
}
