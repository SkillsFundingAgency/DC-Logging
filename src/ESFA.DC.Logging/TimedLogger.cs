using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging
{
    public class TimedLogger : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger _logger;
        private readonly string _message;
        private readonly object[] _parameters;
        private readonly string _callerMemberName;
        private readonly string _callerFilePath;
        private readonly int _callerLineNumber;

        public TimedLogger(
            ILogger logger,
            string message,
            object[] parameters = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            _stopwatch = new Stopwatch();

            _logger = logger;
            _message = message;
            _parameters = parameters;
            _callerMemberName = callerMemberName;
            _callerFilePath = callerFilePath;
            _callerLineNumber = callerLineNumber;

            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            _logger.LogInfo(
                string.Format("{0} - {1}", BuildTimerString(_stopwatch.ElapsedMilliseconds), _message),
                _parameters,
                _callerMemberName,
                _callerFilePath,
                _callerLineNumber);
        }

        public string BuildTimerString(long milliseconds)
        {
            return string.Format("{0} ms", milliseconds);
        }
    }
}
