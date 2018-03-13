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
            var config = new ApplicationLoggerSettings()
            {
                ConnectionString = connectionString
            };

            return new SeriLogging.SeriLogger(config);
        }

        public static ILogger CreateLogger(string connectionString, string jobId)
        {
            var config = new ApplicationLoggerSettings();

            return new SeriLogging.SeriLogger(config, jobId);
        }

        public static ILogger CreateLogger(string connectionString, string jobId, string taskKey)
        {
            var config = new ApplicationLoggerSettings();
            return new SeriLogging.SeriLogger(config, jobId, taskKey);
        }
    }
}
