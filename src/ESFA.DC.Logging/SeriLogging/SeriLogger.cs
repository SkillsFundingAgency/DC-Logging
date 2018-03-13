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
        private readonly IApplicationLoggerSettings _applicationLoggerSettings;
        private readonly Serilog.ILogger _serilogLogger;
        private bool _disposedValue;

        public SeriLogger(IApplicationLoggerSettings applicationLoggerSettings)
        {
            _applicationLoggerSettings = applicationLoggerSettings;

            _serilogLogger = InitializeLogger(applicationLoggerSettings);
        }

        public LoggerConfiguration ConfigureSerilog()
        {
            var seriConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId();

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

        private Serilog.ILogger InitializeLogger(IApplicationLoggerSettings appConfig)
        {
            if (appConfig.EnableInternalLogs)
            {
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Serilog.Debugging.SelfLog.Enable(Console.Error);
            }

            var seriConfig = ConfigureSerilog();

            switch (appConfig.LoggerOutput)
            {
                case Enums.LogOutputDestination.SqlServer:
                    return SqlServerLoggerFactory.CreateLogger(seriConfig, appConfig.ConnectionString, appConfig.LogsTableName);
                case Enums.LogOutputDestination.Console:
                    return ConsoleLoggerFactory.CreateLogger(seriConfig);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Serilog.ILogger AddContext(string callerName, string sourceFile, int lineNumber)
        {
            return _serilogLogger
                .ForContext("CallerName", callerName)
                .ForContext("SourceFile", sourceFile)
                .ForContext("LineNumber", lineNumber)
                .ForContext("JobId", _applicationLoggerSettings.JobId)
                .ForContext("TaskKey", _applicationLoggerSettings.TaskKey);
        }
    }
}