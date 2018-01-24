
using ESFA.DC.Logging.SeriLogging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ESFA.DC.Logging.UnitTests
{
    public class SerilogConfigurationTests
    {
        [Fact]
        public void SetupSeriLogConfig()
        {
            var logger = new SeriLogger(new Mock<ApplicationLoggerSettings>().Object);
            var l = logger.ConfigureSerilog();

            Assert.NotNull(l);
            
        }
    }
}
