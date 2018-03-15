using System;
using System.Runtime.CompilerServices;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ILogger = ESFA.DC.Logging.Interfaces.ILogger;

namespace ESFA.DC.Logging
{
    public class SeriLogger : ILogger
    {
        private readonly Serilog.ILogger _serilogLogger;
        private readonly IExecutionContext _executionContext;

        private bool _disposed;

        public SeriLogger(IApplicationLoggerSettings applicationLoggerSettings, IExecutionContext executionContext, ISerilogLoggerFactory loggerFactory = null)
        {
            loggerFactory = loggerFactory ?? new SerilogLoggerFactory(new LoggerConfigurationBuilder());

            _serilogLogger = loggerFactory.Build(applicationLoggerSettings);

            _executionContext = executionContext;
        }

        public void LogFatal(string message, Exception exception = null, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber, _executionContext.JobId, _executionContext.TaskKey).Fatal(exception, message, parameters);
        }

        public void LogError(string message, Exception ex, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber, _executionContext.JobId, _executionContext.TaskKey).Error(ex, message, parameters);
        }

        public void LogWarning(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber, _executionContext.JobId, _executionContext.TaskKey).Warning(message, parameters);
        }

        public void LogDebug(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber, _executionContext.JobId, _executionContext.TaskKey).Debug(message, parameters);
        }

        public void LogInfo(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber, _executionContext.JobId, _executionContext.TaskKey).Information(message, parameters);
        }

        public void LogVerbose(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber, _executionContext.JobId, _executionContext.TaskKey).Verbose(message, parameters);
        }

        public Serilog.ILogger AddContext(string callerName, string sourceFile, int lineNumber, string jobId, string taskKey)
        {
            return _serilogLogger
                .ForContext("CallerName", callerName)
                .ForContext("SourceFile", sourceFile)
                .ForContext("LineNumber", lineNumber)
                .ForContext("JobId", jobId)
                .ForContext("TaskKey", taskKey);
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
    }
}