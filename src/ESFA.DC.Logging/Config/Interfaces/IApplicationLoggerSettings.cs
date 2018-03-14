using System.Collections;
using System.Collections.Generic;
using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Config.Interfaces
{
    public interface IApplicationLoggerSettings
    {
        string JobId { get; set; }

        string TaskKey { get; set; }

        bool EnableInternalLogs { get; set; }

        IList<IApplicationLoggerOutputSettings> ApplicationLoggerOutputSettingsCollection { get; set; }
    }
}
