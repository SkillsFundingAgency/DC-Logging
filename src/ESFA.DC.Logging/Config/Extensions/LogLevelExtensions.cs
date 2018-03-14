using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.Logging.Enums;
using Serilog.Events;

namespace ESFA.DC.Logging.Config.Extensions
{
    public static class LogLevelExtensions
    {
        public static LogEventLevel ToLogEventLevel(this LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Verbose:
                    return LogEventLevel.Verbose;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Fatal:
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
