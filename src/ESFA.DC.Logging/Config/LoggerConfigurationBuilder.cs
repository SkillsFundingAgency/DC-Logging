using System;
using System.Diagnostics;
using ESFA.DC.Logging.Config.Extensions;
using ESFA.DC.Logging.Config.Interfaces;
using Serilog;

namespace ESFA.DC.Logging.Config
{
    public class LoggerConfigurationBuilder : ILoggerConfigurationBuilder
    {
        public LoggerConfiguration Build(IApplicationLoggerSettings applicationLoggerSettings)
        {
            ConfigureInternalLogs(applicationLoggerSettings.EnableInternalLogs);

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .WithMinimumLogLevel(applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection)
                .WithMsSqlServerSinks(applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection)
                .WithConsoleSinks(applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection);
        }

        private void ConfigureInternalLogs(bool enableInternalLogs)
        {
            if (enableInternalLogs)
            {
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Serilog.Debugging.SelfLog.Enable(Console.Error);
            }
        }
    }
}
