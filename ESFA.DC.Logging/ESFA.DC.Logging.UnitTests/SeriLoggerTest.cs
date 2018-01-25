using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;

using ESFA.DC.Logging.SeriLogging;

namespace ESFA.DC.Logging.UnitTests
{
    
    public class SeriLoggerTest
    {
        

        [Fact]
        public void LoggerInitialisedWithDefaultConnectionString()
        {
            var config = new Moq.Mock<ApplicationLoggerSettings>();
            Assert.NotNull(new SeriLogger(config.Object, "Job1"));
        }

        [Fact]
        public void LoggerInitialisedWithCosoleSettings()
        {
            var config = new Mock<ApplicationLoggerSettings>();
            config.Object.LoggerOutput = Enums.LogOutputDestination.Console;

            Assert.NotNull(new SeriLogger(config.Object,"Job1"));
        }

        [Theory]
        [InlineData(Enums.LogOutputDestination.Console)]
        [InlineData(Enums.LogOutputDestination.SqlServer)]
        public void LoggerErrorDoesNotThrowExceptionForJob(Enums.LogOutputDestination loggerOutputType)
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = loggerOutputType;

            Assert.NotNull(new SeriLogger(config, "Job1"));
        }

        [Theory]
        [InlineData(Enums.LogOutputDestination.Console)]
        [InlineData(Enums.LogOutputDestination.SqlServer)]
        public void LoggerErrorDoesNotThrowExceptionForContext(Enums.LogOutputDestination loggerOutputType)
        {
            var config = new ApplicationLoggerSettings();
            config.LoggerOutput = loggerOutputType;

            Assert.NotNull(new SeriLogger(config, "Job1","Context1"));
        }




    }
}
