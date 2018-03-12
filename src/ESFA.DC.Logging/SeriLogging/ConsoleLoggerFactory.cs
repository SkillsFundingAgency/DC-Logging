using Serilog;
using Serilog.Core;

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
