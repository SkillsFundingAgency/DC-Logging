using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.Logging;
using ESFA.DC.Logging.SeriLogging;

namespace TestConsole
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = new ApplicationLoggerSettings();
            config.ApplicationName = "Test app";
            //config.ConnectionStringKey = "AuditLoggingConnectionString";
            config.LoggerOutput = ESFA.DC.Logging.Enums.LogOutputDestination.SqlServer;

            builder.RegisterType<SeriLogger>().As<ILogger>()
                 .WithParameter(new TypedParameter(typeof(ApplicationLoggerSettings), config));
        }
    }
    }
