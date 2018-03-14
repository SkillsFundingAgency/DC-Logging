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

            ColumnOptions columnOptions = BuildColumnOptions();

            return seriConfig.WriteTo
                .MSSqlServer(connectionStringKey, tableName, autoCreateSqlTable: true, columnOptions: columnOptions)
                .CreateLogger();
        }

        public static ColumnOptions BuildColumnOptions()
        {
            var columnOptions = new ColumnOptions
            {
                TimeStamp =
                {
                    ColumnName = "TimeStampUTC",
                    ConvertToUtc = true,
                },
                AdditionalDataColumns = new Collection<DataColumn>
                {
                    new DataColumn { DataType = typeof(string), ColumnName = "MachineName" },
                    new DataColumn { DataType = typeof(string), ColumnName = "ProcessName" },
                    new DataColumn { DataType = typeof(string), ColumnName = "ThreadId" },
                    new DataColumn { DataType = typeof(string), ColumnName = "CallerName" },
                    new DataColumn { DataType = typeof(string), ColumnName = "SourceFile" },
                    new DataColumn { DataType = typeof(int), ColumnName = "LineNumber" },
                    new DataColumn { DataType = typeof(string), ColumnName = "JobId" },
                    new DataColumn { DataType = typeof(string), ColumnName = "TaskKey" },
                }
            };

            columnOptions.Store.Remove(StandardColumn.Properties);

            return columnOptions;
        }
    }
}
