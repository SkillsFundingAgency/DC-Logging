using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Config
{
    public class ConsoleApplicationLoggerOutputSettings : IConsoleApplicationLoggerOutputSettings
    {
        public LogOutputDestination LoggerOutputDestination
        {
            get { return LogOutputDestination.Console; }
        }

        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Warning;
    }
}
