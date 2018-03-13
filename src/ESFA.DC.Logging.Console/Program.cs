using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging.Console
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static ILogger logger = null;
        static void Main(string[] args)
        {

            var builder = ConfigureBuilder();
            var container = builder.Build();
            var scope = container.BeginLifetimeScope();

            var logger = scope.Resolve<ILogger>();

            //using (var logstuff = new LogStuff(builder))
            //{
            //    logstuff.DoStuff();
            //}

            //    return builder;
            //})

            var config = new ApplicationLoggerSettings();
            config.EnableInternalLogs = true;
            config.JobId = "TestJob";
            //using (var logger = new SeriLogger(config))
            //{

                logger.LogDebug("some debug");
                logger.LogInfo("some info");
                logger.LogWarning("some warn");
                logger.LogError("some error", new Exception("there was an exception"));

            //    logger.StartContext("Testjob2");
            //logger.StartContext("Testjob3withkey","taskKey");
            //logger.LogWarning("some warn with task key");
            //logger.ResetContext();
            //logger.LogWarning("some warn after reset");

            //}

            // ESFA.DC.Logging.LoggerManager
            //logger.Flush();

            // TestStackTraceLevel1();
            //logger.CloseAndFlush();
            System.Console.ReadLine();

        }

        private static ContainerBuilder ConfigureBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<LoggerModule>();


            return builder;
        }

        private static void TestStackTraceLevel1()
        {
            try
            {
                TestStackTraceLevel2();
            }
            catch (Exception ex)
            {
                logger.LogError("There is an error", ex);
            }
            
        }

        private static void TestStackTraceLevel2()
        {
            TestStackTraceLevel3();
        }


        private static void TestStackTraceLevel3()
        {
            try
            {
                var conn = new SqlConnection();
                conn.Open();
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("this is arg null exception",ex); 
            }
        }
    }
}
