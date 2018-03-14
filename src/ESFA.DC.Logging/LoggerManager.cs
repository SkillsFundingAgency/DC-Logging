using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging
{
    public static class LoggerManager
    {
        public static ILogger CreateDefaultLogger()
        {
            var config = new ApplicationLoggerSettings();
            return new SeriLogger(config);
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

            return new SeriLogger(applicationLoggerSettings);
        }
    }
}
