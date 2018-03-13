using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Interfaces
{
    public interface IApplicationLoggerSettings
    {
        LogLevel MinimumLogLevel { get; set; }

        string ConnectionString { get; set; }

        LogOutputDestination LoggerOutput { get; set; }

        string LogsTableName { get; set; }

        bool EnableInternalLogs { get; set; }

        string JobId { get; set; }

        string TaskKey { get; set; }
    }
}
