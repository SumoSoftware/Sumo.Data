using System;

namespace Sumo.Retry
{
    public class RetryOptions
    {
        public RetryOptions(int maxAttempts, TimeSpan timeout)
        {
            if (maxAttempts < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxAttempts));
            }

            if (timeout.TotalMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout));
            }

            MaxAttempts = maxAttempts;
            Timeout = timeout;
        }

        public int MaxAttempts { get; }
        public TimeSpan Timeout { get; }
    }
}
