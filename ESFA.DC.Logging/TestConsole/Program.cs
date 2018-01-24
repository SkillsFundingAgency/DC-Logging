using Autofac;
using ESFA.DC.Logging;
using ESFA.DC.Logging.SeriLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = ConfigureBuilder();
            var container = builder.Build();
            var scope = container.BeginLifetimeScope();
            //using (var scope = container.BeginLifetimeScope())
            //{
              var logger = container.Resolve<ILogger>();


                logger.LogDebug("some debug");
                logger.LogInfo("some info");
                logger.LogWarning("some warn");
                logger.LogError("some error",new Exception("there was an exception"));
                logger.LogInfo("test info {@builder}", parameters:new object[]{builder});
                logger.LogInfo("test info {@builder}, {@jobId}","context1", parameters: new object[] { builder,10 });   
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
    }
}
