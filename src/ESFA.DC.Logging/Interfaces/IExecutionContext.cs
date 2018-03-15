namespace ESFA.DC.Logging.Interfaces
{
    public interface IExecutionContext
    {
        string JobId { get; }

        string TaskKey { get; }
    }
}
