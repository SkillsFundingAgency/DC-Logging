using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using Serilog;
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
                ValidateMsSqlServerApplicationLoggerOutputSettings(msSqlServerApplicationLoggerOutputSettings);

                loggerConfiguration.WriteTo.MSSqlServer(
                    msSqlServerApplicationLoggerOutputSettings.ConnectionString,
                    msSqlServerApplicationLoggerOutputSettings.LogsTableName,
                    msSqlServerApplicationLoggerOutputSettings.MinimumLogLevel.ToLogEventLevel(),
                    autoCreateSqlTable: true,
                    columnOptions: BuildColumnOptions());
            }

            return loggerConfiguration;
        }

        public static void ValidateMsSqlServerApplicationLoggerOutputSettings(IMsSqlServerApplicationLoggerOutputSettings msSqlServerApplicationLoggerOutputSettings)
        {
            if (string.IsNullOrWhiteSpace(msSqlServerApplicationLoggerOutputSettings.ConnectionString))
            {
                throw new ArgumentNullException("There is no ConnectionString defined for SQL Server Logging Sink Configuration.");
            }

            if (string.IsNullOrWhiteSpace(msSqlServerApplicationLoggerOutputSettings.LogsTableName))
            {
                throw new ArgumentNullException("There is no LogsTableName defined for SQL Server Logging Sink Configuration.");
            }
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
