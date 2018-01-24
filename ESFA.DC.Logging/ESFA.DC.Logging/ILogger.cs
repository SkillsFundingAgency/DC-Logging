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
        void LogError(string message, Exception ex, 
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0, 
                            params object[] parameters);
        void LogWarning(string message,
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0, 
                        params object[] parameters);
        void LogDebug(string message,
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0,
                        params object[] parameters);
        void LogInfo(string message, 
                        [CallerMemberName] string callerName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int lineNumber = 0,
                        params object[] parameters);


        

    }
}
