using ESFA.DC.Logging.Enums;
using Serilog;

namespace ESFA.DC.Logging.Config.Extensions
{
    public static class LoggerConfigurationLogLevelExtensions
    {
        public static LoggerConfiguration WithLogLevel(this LoggerConfiguration loggerConfiguration, LogLevel logLevel)
        {
            return loggerConfiguration.MinimumLevel.Is(logLevel.ToLogEventLevel());
        }
    }
}
