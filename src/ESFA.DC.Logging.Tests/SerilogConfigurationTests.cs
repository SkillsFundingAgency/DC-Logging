using ESFA.DC.Logging.SeriLogging;
using Moq;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class SerilogConfigurationTests
    {
        [Fact]
        public void SetupSeriLogConfigForJob()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object);
            var l = logger.ConfigureSerilog();

            Assert.NotNull(l);
        }

        [Fact]
        public void SetupSeriLogConfigForTaskKey()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object);
            var l = logger.ConfigureSerilog();

            Assert.NotNull(l);
        }
    }
}
