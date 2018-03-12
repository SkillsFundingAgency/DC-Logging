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
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object, "Job1");
            var l = logger.ConfigureSerilog();

            Assert.NotNull(l);
        }

        [Fact]
        public void SetupSeriLogConfigForTaskKey()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object, "Job1", "taskkey");
            var l = logger.ConfigureSerilog();

            Assert.NotNull(l);
        }
    }
}
