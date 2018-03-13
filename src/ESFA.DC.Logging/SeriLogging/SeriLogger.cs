using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ESFA.DC.Logging.Interfaces;
using Serilog;
using ILogger = ESFA.DC.Logging.Interfaces.ILogger;

namespace ESFA.DC.Logging.SeriLogging
{
    public class SeriLogger : ILogger
    {
        private Serilog.ILogger _serilogLogger;
        private IApplicationLoggerSettings _applicationLoggerSettings;

        private string _jobId = string.Empty;
        private string _taskKey = string.Empty;
        private bool _disposedValue;

        public SeriLogger(IApplicationLoggerSettings appConfig, string jobId = "", string taskKey = "")
        {
            InitializeLogger(appConfig, jobId, taskKey);
        }

        public LoggerConfiguration ConfigureSerilog()
        {
            var seriConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With<EnvironmentLogEventEnricher>();

            switch (_applicationLoggerSettings.MinimumLogLevel)
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

        public void StartContext(string jobId, string taskKey = "")
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
                    ((IDisposable)_serilogLogger).Dispose();
                }

                _disposedValue = true;
            }
        }

        private void InitializeLogger(IApplicationLoggerSettings appConfig, string jobId, string taskKey)
        {
            if (appConfig.EnableInternalLogs)
            {
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Serilog.Debugging.SelfLog.Enable(Console.Error);
            }

            _applicationLoggerSettings = appConfig;
            _jobId = jobId;
            _taskKey = taskKey;

            var seriConfig = ConfigureSerilog();

            switch (appConfig.LoggerOutput)
            {
                case Enums.LogOutputDestination.SqlServer:
                    _serilogLogger = SqlServerLoggerFactory.CreateLogger(seriConfig, appConfig.ConnectionString, appConfig.LogsTableName);
                    break;
                case Enums.LogOutputDestination.Console:
                    _serilogLogger = ConsoleLoggerFactory.CreateLogger(seriConfig);
                    break;
            }
        }

        private Serilog.ILogger AddContext(string callerName, string sourceFile, int lineNumber)
        {
            return _serilogLogger
                .ForContext("CallerName", callerName)
                .ForContext("SourceFile", sourceFile)
                .ForContext("LineNumber", lineNumber)
                .ForContext("TimeStampUTC", DateTime.Now.ToUniversalTime())
                .ForContext("JobId", _jobId)
                .ForContext("TaskKey", _taskKey);
        }
    }
}