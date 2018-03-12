using System;
using System.Diagnostics;
using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace ESFA.DC.Logging.SeriLogging
{
    public class EnvironmentEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("MachineName", Environment.MachineName));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ProcessName", Process.GetCurrentProcess().ProcessName));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
