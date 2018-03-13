using System;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
