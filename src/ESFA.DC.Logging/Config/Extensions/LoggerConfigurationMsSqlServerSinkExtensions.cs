using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.MSSqlServer;

namespace ESFA.DC.Logging.Config.Extensions
{
    public static class LoggerConfigurationMsSqlServerSinkExtensions
    {
        public static LoggerConfiguration WithMsSqlServerSinks(this LoggerConfiguration loggerConfiguration, IEnumerable<IApplicationLoggerOutputSettings> applicationLoggerOutputSettingsEnumerable)
        {
            foreach (var msSqlServerApplicationLoggerOutputSettings in applicationLoggerOutputSettingsEnumerable
                .Where(s => s.LoggerOutputDestination == LogOutputDestination.SqlServer)
                .Cast<IMsSqlServerApplicationLoggerOutputSettings>())
            {
                if (string.IsNullOrEmpty(msSqlServerApplicationLoggerOutputSettings.ConnectionString))
                {
                    throw new ArgumentNullException("There is no connectionStringKey defined for SQL server logging database");
                }

                loggerConfiguration.WriteTo.MSSqlServer(
                    msSqlServerApplicationLoggerOutputSettings.ConnectionString,
                    msSqlServerApplicationLoggerOutputSettings.LogsTableName,
                    msSqlServerApplicationLoggerOutputSettings.MinimumLogLevel.ToLogEventLevel(),
                    autoCreateSqlTable: true,
                    columnOptions: SetupColumnOptions());
            }

            return loggerConfiguration;
        }

        public static ColumnOptions SetupColumnOptions()
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
