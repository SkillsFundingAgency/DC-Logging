using Autofac;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Logging.SeriLogging;

namespace ESFA.DC.Logging.Console
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = new ApplicationLoggerSettings();

            //config.ConnectionString = "AuditLoggingConnectionString";
            config.LoggerOutput = ESFA.DC.Logging.Enums.LogOutputDestination.SqlServer;

            builder.RegisterType<SeriLogger>().As<ILogger>()
                .WithParameter(new TypedParameter(typeof(ApplicationLoggerSettings), config));

        }
    }
}