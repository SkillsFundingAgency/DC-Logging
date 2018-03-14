using Autofac;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging.Console
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = new ApplicationLoggerSettings
            {
                LoggerOutput = ESFA.DC.Logging.Enums.LogOutputDestination.SqlServer
            };

            builder.RegisterType<SeriLogger>().As<ILogger>()
                .WithParameter(new TypedParameter(typeof(ApplicationLoggerSettings), config));
        }
    }
}