﻿using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging
{
    public class ApplicationLoggerSettings : IApplicationLoggerSettings
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Verbose;

        public string ConnectionString { get; set; } = "AppLogs";

        public LogOutputDestination LoggerOutput { get; set; } = LogOutputDestination.SqlServer;

        public string LogsTableName { get; set; } = "Logs";

        public bool EnableInternalLogs { get; set; }
    }
}
