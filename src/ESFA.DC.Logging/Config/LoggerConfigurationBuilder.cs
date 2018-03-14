using ESFA.DC.Logging.Config.Extensions;
using ESFA.DC.Logging.Config.Interfaces;
using Serilog;

namespace ESFA.DC.Logging.Config
{
    public class LoggerConfigurationBuilder : ILoggerConfigurationBuilder
    {
        public LoggerConfiguration Build(IApplicationLoggerSettings applicationLoggerSettings)
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("JobId", applicationLoggerSettings.JobId)
                .Enrich.WithProperty("TaskKey", applicationLoggerSettings.TaskKey)
                .WithMinimumLogLevel(applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection)
                .WithMsSqlServerSinks(applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection)
                .WithConsoleSinks(applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection);
        }
    }
}
