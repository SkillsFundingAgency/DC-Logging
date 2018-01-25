﻿using Autofac;
using ESFA.DC.Logging;
using ESFA.DC.Logging.SeriLogging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static ILogger logger = null;
        static void Main(string[] args)
        {

            var builder = ConfigureBuilder();
            var container = builder.Build();
            var scope = container.BeginLifetimeScope();
            //using (var scope = container.BeginLifetimeScope())
            //{
              logger = container.Resolve<ILogger>();


                logger.LogDebug("some debug");
                logger.LogInfo("some info");
                logger.LogWarning("some warn");
                logger.LogError("some error",new Exception("there was an exception"));
                logger.LogInfo("test info {@builder}", new object[]{builder});
            //    logger.LogInfo("job id","test info {@builder}",parameters: new object[] { builder });   

            TestStackTraceLevel1();
            //logger.LogWarning("test warn");
            //logger.LogError("test error data {@container}",new Exception("exception occured"), container);
            //}

            //Thread.Sleep(1000);
            Console.ReadLine();

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
