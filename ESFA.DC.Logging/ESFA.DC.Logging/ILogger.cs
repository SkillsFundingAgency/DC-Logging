using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ESFA.DC.Logging
{
    public interface ILogger :IDisposable
    {
        void LogError(string message, Exception ex, object[] parameters= null,
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0);
        void LogWarning(string message, object[] parameters = null,
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0);
        void LogDebug(string message, object[] parameters = null,
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0);
        void LogInfo(string message, object[] parameters = null,
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0);


        

    }
}
