﻿using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging
{
    public static class LoggerManager
    {
        public static ILogger CreateDefaultLogger()
        {
            var config = new ApplicationLoggerSettings();
            var executionContext = new ExecutionContext();
            return new SeriLogger(config, executionContext);
        }

        public static ILogger CreateLogger(string connectionString)
        {
            IApplicationLoggerSettings applicationLoggerSettings = new ApplicationLoggerSettings();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                applicationLoggerSettings.ApplicationLoggerOutputSettingsCollection.Add(
                    new MsSqlServerApplicationLoggerOutputSettings()
                    {
                        ConnectionString = connectionString
                    });
            }

            return new SeriLogger(applicationLoggerSettings, new ExecutionContext());
        }
    }
}
