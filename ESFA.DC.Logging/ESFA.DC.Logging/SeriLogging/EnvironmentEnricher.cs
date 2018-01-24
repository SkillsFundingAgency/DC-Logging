using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.Logging.SeriLogging
{
    class EnvironmentEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
            
            logEvent.AddOrUpdateProperty(pf.CreateProperty("MachineName", Environment.MachineName));
            logEvent.AddOrUpdateProperty(pf.CreateProperty("ProcessName", Process.GetCurrentProcess().ProcessName));
            logEvent.AddOrUpdateProperty(pf.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId));
            

        }
        
    }
}
