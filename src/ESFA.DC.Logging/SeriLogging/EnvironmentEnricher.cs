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
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("MachineName", Environment.MachineName));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ProcessName", Process.GetCurrentProcess().ProcessName));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId));
            

        }
        
    }
}
