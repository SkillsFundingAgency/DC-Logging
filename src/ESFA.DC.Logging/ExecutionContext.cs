using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging
{
    public class ExecutionContext : IExecutionContext
    {
        public string JobId { get; set; }

        public string TaskKey { get; set; }
    }
}
