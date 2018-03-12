using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Core;

namespace ESFA.DC.Logging.SeriLogging
{
    public class SeriLogger : ILogger
    {
        private Logger _logger = null;
        private ApplicationLoggerSettings _appLoggerSettings = null;

        private string _jobId = string.Empty;
        private string _taskKey = string.Empty;
        private bool _disposedValue;

        public SeriLogger(ApplicationLoggerSettings appConfig)
        {
            InitialzeLogger(appConfig, string.Empty, string.Empty);
        }

        public SeriLogger(ApplicationLoggerSettings appConfig, string jobId)
        {
            InitialzeLogger(appConfig, jobId, string.Empty);
        }

        public SeriLogger(ApplicationLoggerSettings appConfig, string jobId, string taskKey)
        {
            InitialzeLogger(appConfig, jobId, taskKey);
        }

        public LoggerConfiguration ConfigureSerilog()
        {
            var seriConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With<EnvironmentEnricher>();

            switch (_appLoggerSettings.MinimumLogLevel)
            {
                case Enums.LogLevel.Verbose:
                    seriConfig.MinimumLevel.Verbose();
                    break;

                case Enums.LogLevel.Debug:
                    seriConfig.MinimumLevel.Debug();
                    break;

                case Enums.LogLevel.Information:
                    seriConfig.MinimumLevel.Information();
                    break;

                case Enums.LogLevel.Warning:
                    seriConfig.MinimumLevel.Warning();
                    break;

                case Enums.LogLevel.Error:
                    seriConfig.MinimumLevel.Error();
                    break;

                case Enums.LogLevel.Fatal:
                    seriConfig.MinimumLevel.Fatal();
                    break;

                default:
                    seriConfig.MinimumLevel.Verbose();
                    break;
            }

            return seriConfig;
        }

        public void LogError(
            string message,
            Exception ex,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Error(ex, message, parameters);
        }

        public void LogWarning(
            string message,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Warning(message, parameters);
        }

        public void LogDebug(
            string message,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Debug(message, parameters);
        }

        public void LogInfo(
            string message,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Information(message, parameters);
        }

        public void StartContext(string jobId)
        {
            _jobId = jobId;
        }

        public void StartContext(string jobId, string taskKey)
        {
            _jobId = jobId;
            _taskKey = taskKey;
        }

        public void ResetContext()
        {
            _jobId = string.Empty;
            _taskKey = string.Empty;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _logger.Dispose();
                }

                _disposedValue = true;
            }
        }

        private void InitialzeLogger(ApplicationLoggerSettings appConfig, string jobId, string taskKey)
        {
            if (appConfig.EnableInternalLogs)
            {
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Serilog.Debugging.SelfLog.Enable(Console.Error);
            }

            _appLoggerSettings = appConfig;
            _jobId = jobId;
            _taskKey = taskKey;

            var seriConfig = ConfigureSerilog();

            if (appConfig.LoggerOutput == Enums.LogOutputDestination.SqlServer)
            {
                _logger = SqlServerLoggerFactory.CreateLogger(seriConfig, appConfig.ConnectionString, appConfig.LogsTableName);
            }
            else if (appConfig.LoggerOutput == Enums.LogOutputDestination.Console)
            {
                _logger = ConsoleLoggerFactory.CreateLogger(seriConfig);
            }
        }

        private Serilog.ILogger AddContext(string callerName, string sourceFile, int lineNumber)
        {
            return _logger.ForContext("CallerName", callerName)
                    .ForContext("SourceFile", sourceFile)
                    .ForContext("LineNumber", lineNumber)
                    .ForContext("TimeStampUTC", DateTime.Now.ToUniversalTime())
                    .ForContext("JobId", _jobId)
                    .ForContext("TaskKey", _taskKey);
        }
    }
}