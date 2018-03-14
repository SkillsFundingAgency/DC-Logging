using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Config.Interfaces
{
    public interface IMsSqlServerApplicationLoggerOutputSettings : IApplicationLoggerOutputSettings
    {
        string ConnectionString { get; set; }

        string LogsTableName { get; set; }
    }
}
