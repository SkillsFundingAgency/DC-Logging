using ESFA.DC.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DC.Logging
{
    public class ApplicationLoggerSettings
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Verbose;
        public string ConnectionStringKey { get; set; } = "AppLogs";
        public LogOutputDestination LoggerOutput { get; set; } = LogOutputDestination.SqlServer;
        public string LogsTableName { get; set; } = "Logs";
        public bool EnableInternalLogs { get; set; }

    }
}
