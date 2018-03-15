using System.Collections.Generic;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Config
{
    public class ApplicationLoggerSettings : IApplicationLoggerSettings
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Verbose;

        public bool EnableInternalLogs { get; set; }

        public string JobId { get; set; }

        public string TaskKey { get; set; }

        public IList<IApplicationLoggerOutputSettings> ApplicationLoggerOutputSettingsCollection { get; set; } = new List<IApplicationLoggerOutputSettings>();
    }
}
