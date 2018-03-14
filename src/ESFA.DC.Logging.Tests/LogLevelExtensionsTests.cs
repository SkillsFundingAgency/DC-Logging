using ESFA.DC.Logging.Config.Extensions;
using ESFA.DC.Logging.Enums;
using FluentAssertions;
using Serilog.Events;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class LogLevelExtensionsTests
    {
        [Theory]
        [InlineData(LogLevel.Verbose, LogEventLevel.Verbose)]
        [InlineData(LogLevel.Debug, LogEventLevel.Debug)]
        [InlineData(LogLevel.Information, LogEventLevel.Information)]
        [InlineData(LogLevel.Warning, LogEventLevel.Warning)]
        [InlineData(LogLevel.Error, LogEventLevel.Error)]
        [InlineData(LogLevel.Fatal, LogEventLevel.Fatal)]
        public void ToLogEventLevel(LogLevel logLevel, LogEventLevel logEventLevel)
        {
            logLevel.ToLogEventLevel().Should().Be(logEventLevel);
        }
    }
}
