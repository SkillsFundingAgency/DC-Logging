using System.Linq;
using ESFA.DC.Logging.Config.Extensions;
using ESFA.DC.Logging.Config.Interfaces;
using Serilog;

namespace ESFA.DC.Logging.Config
{
    public class LoggerConfigurationBuilder : ILoggerConfigurationBuilder
    {
        public LoggerConfiguration Build(IApplicationLoggerSettings applicationLoggerSettings)
        {
            var loggerConfiguration = new LoggerConfiguration();

            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("JobId", applicationLoggerSettings.JobId)
                .Enrich.WithProperty("TaskKey", applicationLoggerSettings.TaskKey);

            if (applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection != null && applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection.Any())
            {
                var minimumLogLevel =
                applicationLoggerSettings
                    .ApplicationLoggerOutputSettingsCollection
                    .Min(s => s.MinimumLogLevel);

                loggerConfiguration.WithLogLevel(minimumLogLevel);

                loggerConfiguration.WithMsSqlServerSinks(applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection);
            }

            return loggerConfiguration;
        }
    }
}
