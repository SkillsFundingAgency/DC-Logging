using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DC.Logging
{
    public static class LoggerManager
    {

        public static ILogger CreateDefaultLogger(string jobId = "", string taskKey = "")
        {
            var config = new ApplicationLoggerSettings();
            return new SeriLogging.SeriLogger(config, jobId, taskKey);
        }
    }
}
