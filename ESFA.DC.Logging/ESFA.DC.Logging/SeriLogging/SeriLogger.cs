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
        private LoggerConfiguration _seriConfig = null;
        

        private ILogEventSink _sink = null;

        #region Constructors
    
        public SeriLogger(ApplicationLoggerSettings appConfig)
        {
            _appLoggerSettings = appConfig;
            _seriConfig = ConfigureSerilog();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            if (appConfig.LoggerOutput == Enums.LogOutputDestination.SqlServer)
            {
              logger =   SqlServerLoggerFactory.CreateLogger(_seriConfig, appConfig.ConnectionStringKey, appConfig.LogsTableName);
            }
            else if (appConfig.LoggerOutput == Enums.LogOutputDestination.Console)
            {
               logger= ConsoleLoggerFactory.CreateLogger(_seriConfig);
            }

            


        }
        public SeriLogger(ApplicationLoggerSettings appConfig, ILogEventSink sink)
        {
            //_appLoggerSettings = appConfig;
            _sink = sink;
            logger = GenericLoggerFactory.CreateLogger(_seriConfig,sink);
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
                    .Enrich.WithEnvironmentUserName() // not sure if this will be useful??
                    .Enrich.With<EnvironmentEnricher>()
                    .Enrich.WithProperty("ApplicationId", _appLoggerSettings.ApplicationName);

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
                            [CallerMemberName] string callerName = "", 
                            [CallerFilePath] string sourceFile = "", 
                            [CallerLineNumber] int lineNumber = 0, 
                            params object[] parameters )
        {
            AddContext(callerName, sourceFile, lineNumber).Error(ex,message,  parameters);
        }
        
        public void LogWarning(string message, 
                            [CallerMemberName] string callerName = "",
                            [CallerFilePath] string sourceFile = "",
                            [CallerLineNumber] int lineNumber = 0, 
                            params object[] parameters)
        {
            AddContext(callerName, sourceFile, lineNumber).Warning(message,parameters);
        }

        public void LogDebug(string message,
                            [CallerMemberName] string callerName = "",
                            [CallerFilePath] string sourceFile = "",
                            [CallerLineNumber] int lineNumber = 0, 
                            params object[] parameters)
        {
            AddContext(callerName, sourceFile, lineNumber).Debug(message,parameters);
        }

        public void LogInfo(string message,
                            [CallerMemberName] string callerName = "",
                            [CallerFilePath] string sourceFile = "",
                            [CallerLineNumber] int lineNumber = 0,
                            params object[] parameters)
        {
            AddContext(callerName, sourceFile, lineNumber).Information(message, parameters);
        }
        

        #endregion

        private Serilog.ILogger AddContext(string callerName , string sourceFile , int lineNumber)
        {
            return logger.ForContext("CallerName", callerName)
                  .ForContext("SourceFile", sourceFile)
                  .ForContext("LineNumber", lineNumber);
                 
        }
        public void Dispose()
        {
            logger.Dispose();
        }

    }
}
