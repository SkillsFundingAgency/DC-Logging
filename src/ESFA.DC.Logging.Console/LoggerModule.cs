using Autofac;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Interfaces;
using System.Collections.Generic;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;

namespace ESFA.DC.Logging.Console
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = new ApplicationLoggerSettings
            {
                ApplicationLoggerOutputSettingsCollection = new List<IApplicationLoggerOutputSettings>()
                {
                    new MsSqlServerApplicationLoggerOutputSettings()
                    {
                        MinimumLogLevel = LogLevel.Verbose
                    },
                    new ConsoleApplicationLoggerOutputSettings()
                    {
                        MinimumLogLevel = LogLevel.Verbose
                    }
                }
            };
            
            builder.RegisterInstance(config).As<IApplicationLoggerSettings>().SingleInstance();
            builder.RegisterType<ExecutionContext>().As<IExecutionContext>().InstancePerLifetimeScope();
            builder.RegisterType<SerilogLoggerFactory>().As<ISerilogLoggerFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SeriLogger>().As<ILogger>().InstancePerLifetimeScope();
        }
    }
}