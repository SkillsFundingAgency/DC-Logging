using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Logging.SeriLogging;
using Serilog;
using ILogger = ESFA.DC.Logging.Interfaces.ILogger;

namespace ESFA.DC.Logging
{
    public class SeriLogger : ILogger
    {
        private readonly IApplicationLoggerSettings _applicationLoggerSettings;
        private readonly ILoggerConfigurationBuilder _loggerConfigurationBuilder;
        private readonly Serilog.ILogger _serilogLogger;
        private bool _disposed;

        public SeriLogger(IApplicationLoggerSettings applicationLoggerSettings, ILoggerConfigurationBuilder loggerConfigurationBuilder = null)
        {
            _applicationLoggerSettings = applicationLoggerSettings;
            _loggerConfigurationBuilder = loggerConfigurationBuilder ?? new LoggerConfigurationBuilder();

            _serilogLogger = InitializeLogger();
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
            if (!_disposed)
            {
                if (disposing)
                {
                    ((IDisposable)_serilogLogger).Dispose();
                }

                _disposed = true;
            }
        }

        private Serilog.ILogger InitializeLogger()
        {
            if (_applicationLoggerSettings.EnableInternalLogs)
            {
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Serilog.Debugging.SelfLog.Enable(Console.Error);
            }

            return _loggerConfigurationBuilder.Build(_applicationLoggerSettings).CreateLogger();

            ////switch (applicationLoggerSettings.LoggerOutput)
            ////{
            ////    case Enums.LogOutputDestination.SqlServer:
            ////        return SqlServerLoggerFactory.CreateLogger(seriConfig, applicationLoggerSettings.ConnectionString, applicationLoggerSettings.LogsTableName);
            ////    case Enums.LogOutputDestination.Console:
            ////        return ConsoleLoggerFactory.CreateLogger(seriConfig);
            ////    default:
            ////        throw new ArgumentOutOfRangeException();
            ////}
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