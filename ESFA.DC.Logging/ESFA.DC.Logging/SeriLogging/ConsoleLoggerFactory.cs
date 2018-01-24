using ESFA.DC.Logging.Enums;
using Serilog;
using Serilog.Core;
using System;

namespace ESFA.DC.Logging.SeriLogging
{
    public class ConsoleLoggerFactory
    {
        public static Logger CreateLogger(LoggerConfiguration seriConfig)
        {
            
            return seriConfig.WriteTo
                .Console()
                .CreateLogger();

        }

    }
}
