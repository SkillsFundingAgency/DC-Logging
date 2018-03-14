using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ILogger = Serilog.ILogger;

namespace ESFA.DC.Logging
{
    public class SerilogLoggerFactory : ISerilogLoggerFactory
    {
        private readonly ILoggerConfigurationBuilder _loggerConfigurationBuilder;

        public SerilogLoggerFactory(ILoggerConfigurationBuilder loggerConfigurationBuilder = null)
        {
            _loggerConfigurationBuilder = loggerConfigurationBuilder ?? new LoggerConfigurationBuilder();
        }

        public ILogger Build(IApplicationLoggerSettings applicationLoggerSettings)
        {
            return _loggerConfigurationBuilder.Build(applicationLoggerSettings).CreateLogger();
        }
    }
}
