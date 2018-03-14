using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Config.Interfaces
{
    public interface IApplicationLoggerOutputSettings
    {
        LogOutputDestination LoggerOutputDestination { get; }

        LogLevel MinimumLogLevel { get; set; }
    }
}
