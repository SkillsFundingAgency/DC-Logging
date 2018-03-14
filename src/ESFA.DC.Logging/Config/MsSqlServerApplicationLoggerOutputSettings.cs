using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Config
{
    public class MsSqlServerApplicationLoggerOutputSettings : IMsSqlServerApplicationLoggerOutputSettings, IApplicationLoggerOutputSettings
    {
        public LogOutputDestination LoggerOutputDestination
        {
            get { return LogOutputDestination.SqlServer; }
        }

        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Warning;

        public string ConnectionString { get; set; } = "AppLogs";

        public string LogsTableName { get; set; } = "Logs";
    }
}
