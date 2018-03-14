using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ILogger = ESFA.DC.Logging.Interfaces.ILogger;

namespace ESFA.DC.Logging
{
    public class SeriLogger : ILogger
    {
        private readonly Serilog.ILogger _serilogLogger;
        private bool _disposed;

        public SeriLogger(IApplicationLoggerSettings applicationLoggerSettings, ILoggerConfigurationBuilder loggerConfigurationBuilder = null)
        {
            var loggerConfigurationBuilder1 = loggerConfigurationBuilder ?? new LoggerConfigurationBuilder();

            _serilogLogger = loggerConfigurationBuilder1.Build(applicationLoggerSettings).CreateLogger();

            ConfigureInternalLogs(applicationLoggerSettings.EnableInternalLogs);
        }

        public void LogFatal(string message, Exception exception = null, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber).Fatal(exception, message, parameters);
        }

        public void LogError(string message, Exception ex, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber).Error(ex, message, parameters);
        }

        public void LogWarning(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber).Warning(message, parameters);
        }

        public void LogDebug(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber).Debug(message, parameters);
        }

        public void LogInfo(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber).Information(message, parameters);
        }

        public void LogVerbose(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            AddContext(callerMemberName, callerFilePath, callerLineNumber).Verbose(message, parameters);
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

        private void ConfigureInternalLogs(bool enableInternalLogs)
        {
            if (enableInternalLogs)
            {
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Serilog.Debugging.SelfLog.Enable(Console.Error);
            }
        }

        private Serilog.ILogger AddContext(string callerName, string sourceFile, int lineNumber)
        {
            return _serilogLogger
                .ForContext("CallerName", callerName)
                .ForContext("SourceFile", sourceFile)
                .ForContext("LineNumber", lineNumber);
        }
    }
}