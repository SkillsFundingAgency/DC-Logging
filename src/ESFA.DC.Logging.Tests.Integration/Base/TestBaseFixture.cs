using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Dapper;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Logging.SeriLogging;
using ESFA.DC.Logging.Tests.Integration.Entities;

namespace ESFA.DC.Logging.Tests.Integration.Base
{
    [ExcludeFromCodeCoverage]
    public class TestBaseFixture : IDisposable
    {
        private readonly string _connectionString;
        private readonly DatabaseHelper _databaseHelper;

        public TestBaseFixture()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AppLogs"].ConnectionString;
            _databaseHelper = new DatabaseHelper(_connectionString);

            _databaseHelper.CreateIfNotExists();
        }

        public void Dispose()
        {
            _databaseHelper.DropIfExists();
        }

        public bool CheckIfTableExists(string tableName)
        {
            return _databaseHelper.CheckIfTableExists("Logs", _connectionString);
        }

        public ILogger CreateLogger(LogLevel logLevel, string jobId = "", string taskKey = "")
        {
            DeleteLogs();

            var config = new ApplicationLoggerSettings
            {
                MinimumLogLevel = logLevel,
                JobId = jobId,
                TaskKey = taskKey,
            };

            return new SeriLogger(config);
        }

        public List<AppLogEntity> GetLogs()
        {
            List<AppLogEntity> result = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                result = connection.Query<AppLogEntity>("SELECT * FROM Logs").ToList();
                connection.Close();
            }

            return result;
        }

        public void DeleteLogs()
        {
            if (CheckIfTableExists("Logs"))
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute("DELETE FROM Logs");
                }
            }
        }
    }
}
