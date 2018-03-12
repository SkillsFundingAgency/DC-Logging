using System;
using System.Collections.ObjectModel;
using System.Data;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;

namespace ESFA.DC.Logging.SeriLogging
{
    public static class SqlServerLoggerFactory
    {
        public static Logger CreateLogger(LoggerConfiguration seriConfig, string connectionStringKey, string tableName)
        {
            if (string.IsNullOrEmpty(connectionStringKey))
            {
                throw new ArgumentNullException("There is no connectionStringKey defined for SQL server logging database");
            }

            ColumnOptions columnOptions = SetupColumnOptions();

            return seriConfig.WriteTo
                .MSSqlServer(connectionStringKey, tableName, autoCreateSqlTable: true, columnOptions: columnOptions)
                .CreateLogger();
        }

        public static ColumnOptions SetupColumnOptions()
        {
            var columnOptions = new ColumnOptions
            {
                AdditionalDataColumns = new Collection<DataColumn>
                {
                    new DataColumn { DataType = typeof(DateTime), ColumnName = "TimeStampUTC" },
                    new DataColumn { DataType = typeof(string), ColumnName = "MachineName" },
                    new DataColumn { DataType = typeof(string), ColumnName = "ProcessName" },
                    new DataColumn { DataType = typeof(string), ColumnName = "CallerName" },
                    new DataColumn { DataType = typeof(string), ColumnName = "SourceFile" },
                    new DataColumn { DataType = typeof(int), ColumnName = "LineNumber" },
                    new DataColumn { DataType = typeof(string), ColumnName = "JobId" },
                    new DataColumn { DataType = typeof(string), ColumnName = "TaskKey" },
                }
            };

            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.TimeStamp);

            return columnOptions;
        }
    }
}
