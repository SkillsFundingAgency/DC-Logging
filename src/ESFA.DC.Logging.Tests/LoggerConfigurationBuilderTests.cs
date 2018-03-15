using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class LoggerConfigurationBuilderTests
    {
        [Fact]
        public void Build_NotNull()
        {
            var config = new LoggerConfigurationBuilder().Build(new Mock<IApplicationLoggerSettings>().Object);

            config.Should().NotBeNull();
        }
    }
}
