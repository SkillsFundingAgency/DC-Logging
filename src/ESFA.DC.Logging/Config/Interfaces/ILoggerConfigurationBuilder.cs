using Serilog;

namespace ESFA.DC.Logging.Config.Interfaces
{
    public interface ILoggerConfigurationBuilder
    {
        LoggerConfiguration Build(IApplicationLoggerSettings applicationLoggerSettings);
    }
}
