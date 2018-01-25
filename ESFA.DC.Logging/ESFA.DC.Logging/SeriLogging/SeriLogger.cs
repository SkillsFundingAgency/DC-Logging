using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using System.Collections.ObjectModel;
using System.Data;
using Serilog.Context;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ESFA.DC.Logging.SeriLogging
{
    public class SeriLogger : ILogger
    {
        private Logger logger = null;
        private ApplicationLoggerSettings _appLoggerSettings = null;
      

        private string _jobId = string.Empty;
        private string _taskKey = string.Empty;
        
      
        #region Constructors

        public SeriLogger(ApplicationLoggerSettings appConfig, string jobId)
        {
            InitialzeLogger(appConfig, jobId, string.Empty);
        }

        public SeriLogger(ApplicationLoggerSettings appConfig, string jobId,string taskKey)
        {
            InitialzeLogger(appConfig, jobId, taskKey);
        }

        private void InitialzeLogger(ApplicationLoggerSettings appConfig, string jobId, string taskKey)
        {
            _appLoggerSettings = appConfig;
            _jobId = jobId;
            _taskKey = taskKey;

            var seriConfig = ConfigureSerilog();
          
   
            if (appConfig.LoggerOutput == Enums.LogOutputDestination.SqlServer)
            {
              logger =   SqlServerLoggerFactory.CreateLogger(seriConfig, appConfig.ConnectionStringKey, appConfig.LogsTableName);
            }
            else if (appConfig.LoggerOutput == Enums.LogOutputDestination.Console)
            {
               logger= ConsoleLoggerFactory.CreateLogger(seriConfig);
            }

        }
      

        #endregion

        #region Public Methods
       

        /// <summary>
        /// Creates the logger configuration for serilog
        /// </summary>
        /// <param name="appConfig"></param>
        /// <returns></returns>
        public LoggerConfiguration ConfigureSerilog()
        {

            //setup the configuartion
            var seriConfig = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.With<EnvironmentEnricher>()
                    .Enrich.WithProperty("ApplicationId", _appLoggerSettings.ApplicationName)
                    .Enrich.WithProperty("JobId", _jobId)
                    .Enrich.WithProperty("TaskKey", _taskKey);

            switch (_appLoggerSettings.MinimumLogLevel)
            {
                case Enums.LogLevel.Verbose:
                    seriConfig.MinimumLevel.Verbose();
                    break;
                case Enums.LogLevel.Debug:
                    seriConfig.MinimumLevel.Debug();
                    break;
                case Enums.LogLevel.Information:
                    seriConfig.MinimumLevel.Information();
                    break;
                case Enums.LogLevel.Warning:
                    seriConfig.MinimumLevel.Warning();
                    break;
                case Enums.LogLevel.Error:
                    seriConfig.MinimumLevel.Error();
                    break;
                case Enums.LogLevel.Fatal:
                    seriConfig.MinimumLevel.Fatal();
                    break;
                default:
                    seriConfig.MinimumLevel.Verbose();
                    break;
            }

            return seriConfig;
        }


        #endregion

        #region Logger functions
        public void LogError(string message, 
                            Exception ex,
                            object[] parameters = null,
                            [CallerMemberName] string callerName = "", 
                            [CallerFilePath] string sourceFile = "", 
                            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Error(ex,message,  parameters);
        }
        
        public void LogWarning(string message,
                            object[] parameters = null,
                            [CallerMemberName] string callerName = "",
                            [CallerFilePath] string sourceFile = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Warning(message,parameters);
        }

        public void LogDebug(string message, object[] parameters = null,
                            [CallerMemberName] string callerName = "",
                            [CallerFilePath] string sourceFile = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Debug(message,parameters);
        }

        public void LogInfo(string message, object[] parameters = null,
                            [CallerMemberName] string callerName = "",
                            [CallerFilePath] string sourceFile = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            AddContext(callerName, sourceFile, lineNumber).Information(message, parameters);
        }
        

        #endregion

        private Serilog.ILogger AddContext(string callerName , string sourceFile , int lineNumber)
        {
            return logger.ForContext("CallerName", callerName)
                  .ForContext("SourceFile", sourceFile)
                  .ForContext("LineNumber", lineNumber)
                  .ForContext("TimeStampUTC", DateTime.Now.ToUniversalTime());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    logger.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
