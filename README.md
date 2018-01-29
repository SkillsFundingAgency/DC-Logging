# DC Logging

Repository to capture logs generated from applications. This repository holds source code which is published as a nuget package. This repository relies on Serilog as third party component for logging.

There are multiple options for log storage and these can be configured on **ApplicationLoggerSettings** which is passed in as a parameter to the constructor;

###### Minimum Setup Required
1. For Console logger - None
2. For SQL server logger - This component relies on an empty database and connection string defined in the client application.

###### ApplicationLoggerSettings

	
  * LogLevel **MinimumLogLevel** Default is set to Verbose
  * string **ConnectionStringKey** Default is set to AppLogs
  * LogOutputDestination **LoggerOutput**  Default is set to LogOutputDestination.SqlServer  Other option is Console
  * string **LogsTableName**  Table name which will be created by component to store logs   Default is set to  Logs
  * bool **EnableInternalLogs**  Default is set to ''false'' Can be set to true and internal logs will be displayed in console
	 
	

Each instance of logger needs to be disposed off correctly in order to ensure that all the logs are written correctly as logger keeps a buffer and does not write each time you call LogInfor or LogError.

###### Usage
Create an instance of ILogger or configure your Ioc to pass that into your class. There are 3 overloads for constructor and logger can be initiated for a particlaur job/task key and each log entry will be created with that jobid/taskkey so each entry can be traced to individual job.
```
var config = new ESFA.DC.Logging.ApplicationLoggerSettings();
using (var logger new ESFA.DC.Logging.SeriLogging.SeriLogger(config, "jobId","taskKey"))
{
  logger.LogDebug("some debug");
  logger.LogInfo("some info");
  logger.LogWarning("some warn");
  logger.LogError("some error", new Exception("there was an exception"));
}
```
