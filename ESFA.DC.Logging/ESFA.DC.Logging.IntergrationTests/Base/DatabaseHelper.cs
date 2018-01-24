using Dapper;
using ESFA.DC.Logging.IntergrationTests.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DC.Logging.IntergrationTests
{
    public class DatabaseHelper
    {
        public string MasterConnectionString { get; protected set; }
        public string AppConnectionString { get; protected set; }

        public DatabaseHelper(string connectionString)
        {
            
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            AppConnectionString = connectionStringBuilder.ToString();

            var databaseName = connectionStringBuilder.InitialCatalog;
            connectionStringBuilder.InitialCatalog = "master";

            MasterConnectionString = connectionStringBuilder.ToString();

        }


        public bool CheckIfTableExists(string tableName, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format($"select * from information_schema.tables where table_name = '{tableName}'", tableName);
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
                    command.CommandText = string.Format("select * from master.dbo.sysdatabases where name='AppLogs'");
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                 
                }
            }
        }

        private void ExecuteCommand(string commandText)
        {
            using (var connection = new SqlConnection(MasterConnectionString))
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
                ExecuteCommand("CREATE DATABASE AppLogs" );
        }

        public void DropIfExists()
        {

            if (CheckIfDatabaseExists())
            {
                ExecuteCommand("ALTER DATABASE AppLogs SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                ExecuteCommand("DROP DATABASE AppLogs");
            }
            
        }

       
    }
}
