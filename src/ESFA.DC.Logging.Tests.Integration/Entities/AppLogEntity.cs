using System;
using System.Diagnostics.CodeAnalysis;

namespace ESFA.DC.Logging.IntergrationTests.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class AppLogEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime TimeStampUTC { get; set; }
        public string Exception { get; set; }
        
        public string MachineName { get; set; }
        public string ProcessName { get; set; }
        public string CallerName { get; set; }
        public string SourceFile { get; set; }
        public string LineNumber { get; set; }
        public string JobId { get; set; }
        public string TaskKey { get; set; }


    }
}
