using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace ESFA.DC.Logging.IntergrationTests
{
    [ExcludeFromCodeCoverageAttribute]
    public class DatabaseHelper
    {
        public string MasterConnectionString { get; protected set; }
        public string AppConnectionString { get; protected set; }
        private bool _isDatabaseCratedByTests = false;

        public DatabaseHelper(string connectionString)
        {
            
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            AppConnectionString = connectionStringBuilder.ToString();

            connectionStringBuilder.InitialCatalog = "master";

            MasterConnectionString = connectionStringBuilder.ToString();

        }


        [SuppressMessage("Security", "S3649")]
        public bool CheckIfTableExists(string tableName, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    
                    command.CommandText = $"select * from information_schema.tables where table_name = '{tableName}'";
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }

                }
            }
        }
            

        private bool CheckIfDatabaseExists()
        {
            
            using (var connection = new SqlConnection(MasterConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from master.dbo.sysdatabases where name='AppLogs'";
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                 
                }
            }
        }

        private void ExecuteCommand(string commandText,string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

     

        public void CreateIfNotExists()
        {
            if (!CheckIfDatabaseExists())
            {
                ExecuteCommand("CREATE DATABASE AppLogs",MasterConnectionString);
                _isDatabaseCratedByTests = true;
            }
            else
            {
                ExecuteCommand("DROP TABLE dbo.Logs",AppConnectionString);
            }


        }

        public void DropIfExists()
        {

            if (CheckIfDatabaseExists() && _isDatabaseCratedByTests)
            {
                ExecuteCommand("ALTER DATABASE AppLogs SET SINGLE_USER WITH ROLLBACK IMMEDIATE",MasterConnectionString);
                ExecuteCommand("DROP DATABASE AppLogs",MasterConnectionString);
            }
            
        }

       
    }
}
