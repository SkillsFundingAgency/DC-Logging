using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Autofac;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging.Console
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            var builder = ConfigureBuilder();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var executionContext = (ExecutionContext) scope.Resolve<IExecutionContext>();

                executionContext.JobId = "TestJobId";
                executionContext.TaskKey = "TestTaskKey";

                var logger = scope.Resolve<ILogger>();

                logger.LogDebug("some debug");
                logger.LogInfo("some info");
                logger.LogWarning("some warn");

                try
                {
                    throw new AccessViolationException("No Access");
                }
                catch (Exception e)
                {
                    logger.LogError("some error", e);
                }
                
                using (var timedLogger = new TimedLogger(logger, "test Timer"))
                {
                    Thread.Sleep(1234);
                }
            }

            System.Console.ReadLine();
        }

        private static ContainerBuilder ConfigureBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<LoggerModule>();

            return builder;
        }
    }
}
