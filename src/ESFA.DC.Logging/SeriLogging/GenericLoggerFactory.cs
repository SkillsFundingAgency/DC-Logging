using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
