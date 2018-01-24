using Dapper;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.IntergrationTests.Models;
using ESFA.DC.Logging.SeriLogging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ESFA.DC.Logging.IntergrationTests
{
    public  class TestBaseFixture : IDisposable
    {
        public string ConnectionString = "";
        private DatabaseHelper _databaseHelper = null;

        public TestBaseFixture()
        {

            ConnectionString = ConfigurationManager.ConnectionStrings["AppLogs"].ConnectionString;
            _databaseHelper = new DatabaseHelper(ConnectionString);

            _databaseHelper.CreateIfNotExists();

        }
        
        public void Dispose()
        {
            _databaseHelper.DropIfExists();
        }

        public bool CheckIfTableExists(string tableName)
        {
            return _databaseHelper.CheckIfTableExists("Logs", ConnectionString);
        }


        public ILogger CreateLogger(LogLevel logLevel = LogLevel.Verbose)
        {
            DeleteLogs();

            var config = new ApplicationLoggerSettings();
            config.ApplicationName = "Test App";
            config.MinimumLogLevel = logLevel;
            return new SeriLogger(config);
        }


        public List<AppLogEntity> GetLogs()
        {
            List<AppLogEntity> result = null;
            using (var connection = new SqlConnection(ConnectionString))
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
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Execute("DELETE FROM Logs");

                }
            }
        }



    }
}
