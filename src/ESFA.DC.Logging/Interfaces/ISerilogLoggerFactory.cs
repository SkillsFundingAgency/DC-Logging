using ESFA.DC.Logging.Config.Interfaces;

namespace ESFA.DC.Logging.Interfaces
{
    public interface ISerilogLoggerFactory
    {
        Serilog.ILogger Build(IApplicationLoggerSettings applicationLoggerSettings);
    }
}
