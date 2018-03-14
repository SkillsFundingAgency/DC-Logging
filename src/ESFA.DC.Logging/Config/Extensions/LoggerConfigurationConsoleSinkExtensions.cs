using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using Serilog;

namespace ESFA.DC.Logging.Config.Extensions
{
    public static class LoggerConfigurationConsoleSinkExtensions
    {
        public static LoggerConfiguration WithConsoleSinks(this LoggerConfiguration loggerConfiguration, IEnumerable<IApplicationLoggerOutputSettings> applicationLoggerOutputSettingsEnumerable)
        {
            if (applicationLoggerOutputSettingsEnumerable != null)
            {
                foreach (var consoleApplicationLoggerOutputSettings in applicationLoggerOutputSettingsEnumerable
                    .Where(s => s.LoggerOutputDestination == LogOutputDestination.Console)
                    .Cast<IConsoleApplicationLoggerOutputSettings>())
                {
                    loggerConfiguration.WriteTo.Console(consoleApplicationLoggerOutputSettings.MinimumLogLevel
                        .ToLogEventLevel());
                }
            }

            return loggerConfiguration;
        }
    }
}
