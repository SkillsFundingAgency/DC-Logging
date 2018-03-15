using FluentAssertions;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class LoggerManagerTests
    {
        [Fact]
        public void TestDefaultLogger()
        {
            var logger = LoggerManager.CreateDefaultLogger();

            logger.Should().NotBeNull();
            logger.Should().BeOfType<SeriLogger>();
        }

        [Fact]
        public void TestConnectionStringLogger()
        {
            var logger = LoggerManager.CreateLogger("test connection string");

            logger.Should().NotBeNull();
            logger.Should().BeOfType<SeriLogger>();
        }
    }
}
