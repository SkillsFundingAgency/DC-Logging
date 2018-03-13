using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging
{
    public static class LoggerManager
    {
        public static ILogger CreateDefaultLogger()
        {
            var config = new ApplicationLoggerSettings();
            return new SeriLogging.SeriLogger(config);
        }

        public static ILogger CreateLogger(string connectionString)
        {
            IApplicationLoggerSettings applicationLoggerSettings = new ApplicationLoggerSettings();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                applicationLoggerSettings.ConnectionString = connectionString;
            }

            return new SeriLogging.SeriLogger(applicationLoggerSettings);
        }
    }
}
