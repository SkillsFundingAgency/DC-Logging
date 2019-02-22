using System;
using System.Runtime.CompilerServices;

namespace ESFA.DC.Logging.Interfaces
{
    public interface ILogger : IDisposable
    {
        void LogFatal(
            string message,
            Exception exception = null,
            object[] parameters = null,
            int jobIdOverride = -1,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0);

        void LogError(
            string message,
            Exception exception = null,
            object[] parameters = null,
            int jobIdOverride = -1,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0);

        void LogWarning(
            string message,
            object[] parameters = null,
            int jobIdOverride = -1,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0);

        void LogDebug(
            string message,
            object[] parameters = null,
            int jobIdOverride = -1,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0);

        void LogInfo(
            string message,
            object[] parameters = null,
            int jobIdOverride = -1,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0);

        void LogVerbose(
            string message,
            object[] parameters = null,
            int jobIdOverride = -1,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0);
    }
}