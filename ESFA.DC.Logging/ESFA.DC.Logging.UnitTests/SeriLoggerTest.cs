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
            Assert.NotNull(new SeriLogger(config.Object));
        }

        [Fact]
        public void LoggerInitialisedWithCosoleSettings()
        {
            var config = new Mock<ApplicationLoggerSettings>();
            config.Object.LoggerOutput = Enums.LogOutputDestination.Console;

            Assert.NotNull(new SeriLogger(config.Object));
        }

        [Fact]
        public void LoggerErrorDoesNotThrowException()
        {
            var config = new ApplicationLoggerSettings();
            config.ApplicationName = "Test";
            config.LoggerOutput = Enums.LogOutputDestination.Console;

            Assert.NotNull(new SeriLogger(config));
        }


        [Fact]
        public void LoggerErrorTest()
        {
            var config = new ApplicationLoggerSettings();
            config.ApplicationName = "Test";
            config.LoggerOutput = Enums.LogOutputDestination.Console;

            Assert.NotNull(new SeriLogger(config));
        }
    }
}
