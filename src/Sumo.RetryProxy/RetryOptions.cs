using System;

namespace Sumo.Retry
{
    public class RetryOptions
    {
        public RetryOptions(int maxAttempts, TimeSpan timeout)
        {
            MaxAttempts = maxAttempts;
            Timeout = timeout;
        }

        public int MaxAttempts { get; }
        public TimeSpan Timeout { get; }
    }
}
