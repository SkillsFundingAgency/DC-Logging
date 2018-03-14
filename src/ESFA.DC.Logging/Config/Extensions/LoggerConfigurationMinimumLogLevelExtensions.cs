using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Logging.Config.Interfaces;
using Serilog;

namespace ESFA.DC.Logging.Config.Extensions
{
    public static class LoggerConfigurationMinimumLogLevelExtensions
    {
        public static LoggerConfiguration WithMinimumLogLevel(this LoggerConfiguration loggerConfiguration, IEnumerable<IApplicationLoggerOutputSettings> applicationLoggerOutputSettingsEnumerable)
        {
            if (applicationLoggerOutputSettingsEnumerable != null)
            {
                var enumeratedSettings = applicationLoggerOutputSettingsEnumerable.ToList();

                if (enumeratedSettings.Any())
                {
                    var minimumLogLevel = enumeratedSettings.Min(s => s.MinimumLogLevel);

                    loggerConfiguration.MinimumLevel.Is(minimumLogLevel.ToLogEventLevel());
                }
            }

            return loggerConfiguration;
        }
    }
}
