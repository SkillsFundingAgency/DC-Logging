using Serilog;
using Serilog.Core;

namespace ESFA.DC.Logging.SeriLogging
{
    public class GenericLoggerFactory
    {
        public static Logger CreateLogger(LoggerConfiguration seriConfig, ILogEventSink sink)
        {
            return seriConfig
                .WriteTo
                .Sink(sink)
                .CreateLogger();
        }
    }
}
