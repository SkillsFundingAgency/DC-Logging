using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dapper;
using ESFA.DC.Logging.Tests.Integration.Model;

namespace ESFA.DC.Logging.Tests.Integration.Fixtures
{
    [ExcludeFromCodeCoverage]
    public class SqlDatabaseFixture : IDisposable
    {
        private const string LogsTableName = "Logs";
        private const string DatabaseName = "AppLogs";
        private readonly string _connectionString;
        private readonly string _masterConnectionString;

        public SqlDatabaseFixture()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AppLogs"].ConnectionString;

            var connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = "master"
            };

            _masterConnectionString = connectionStringBuilder.ToString();

            DropIfExists();
            CreateIfNotExists();
        }

        public void Dispose()
        {
            DropIfExists();
        }

        public IEnumerable<T> Get<T>()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<T>($"SELECT * FROM {LogsTableName}");
            }
        }

        public void TruncateLogs()
        {
            if (CheckIfTableExists(LogsTableName))
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute($"TRUNCATE TABLE {LogsTableName}");
                }
            }
        }

        private void CreateIfNotExists()
        {
            if (!CheckIfDatabaseExists())
            {
                ExecuteSql($"CREATE DATABASE {DatabaseName}", _masterConnectionString);
            }
        }

        private void DropIfExists()
        {
            if (CheckIfDatabaseExists())
            {
                ExecuteSql($"ALTER DATABASE {DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", _masterConnectionString);
                ExecuteSql($"DROP DATABASE {DatabaseName}", _masterConnectionString);
            }
        }

        private bool CheckIfDatabaseExists()
        {
            using (var connection = new SqlConnection(_masterConnectionString))
            {
                connection.Open();

                return connection.Query($"SELECT * from dbo.sysdatabases WHERE name ='{DatabaseName}'").Any();
            }
        }

        private bool CheckIfTableExists(string tableName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query($"SELECT * FROM information_schema.tables WHERE table_name = '{tableName}'").Any();
            }
        }

        private void ExecuteSql(string sql, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.Execute(sql);
            }
        }
    }
}
