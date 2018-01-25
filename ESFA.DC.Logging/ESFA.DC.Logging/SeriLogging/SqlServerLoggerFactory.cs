using ESFA.DC.Logging.Enums;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Creates column configs for seri log
        /// </summary>
        /// <returns></returns>
        public static ColumnOptions SetupColumnOptions()
        {
            //Set the aditional colum which we need to be added as part of logs table
            var columnOptions = new ColumnOptions();
            columnOptions.AdditionalDataColumns = new Collection<DataColumn>{
                        new DataColumn { DataType = typeof(DateTime), ColumnName = "TimeStampUTC" },
                        new DataColumn { DataType = typeof(string), ColumnName = "ApplicationId" },
                         new DataColumn { DataType = typeof(string), ColumnName = "MachineName" },
                         new DataColumn { DataType = typeof(string), ColumnName = "ProcessName" },
                         new DataColumn { DataType = typeof(string), ColumnName = "CallerName" },
                        new DataColumn { DataType = typeof(string), ColumnName = "SourceFile" },
                        new DataColumn { DataType = typeof(int), ColumnName = "LineNumber" },
                        new DataColumn { DataType = typeof(string), ColumnName = "JobId" },
                        new DataColumn { DataType = typeof(string), ColumnName = "TaskKey" },

                    };

            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.TimeStamp);



            return columnOptions;
        }
    }
}
