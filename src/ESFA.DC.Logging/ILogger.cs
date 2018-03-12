using System;
using System.Runtime.CompilerServices;

namespace ESFA.DC.Logging
{
    public interface ILogger : IDisposable
    {
        void LogError(
            string message,
            Exception ex = null,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0);

        void LogWarning(
            string message,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0);

        void LogDebug(
            string message,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0);

        void LogInfo(
            string message,
            object[] parameters = null,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int lineNumber = 0);

        void StartContext(string jobId);

        void StartContext(string jobId, string taskKey);

        void ResetContext();
    }
}