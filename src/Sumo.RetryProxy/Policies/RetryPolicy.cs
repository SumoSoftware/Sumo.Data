using System;

namespace Sumo.Retry.Policies
{
    public class RetryPolicy
    {
        public RetryPolicy(RetryPolicy retryPolicy)
        {
            MaxAttempts = retryPolicy.MaxAttempts;
            Timeout = TimeSpan.FromTicks(retryPolicy.Timeout.Ticks);
            InitialInterval = TimeSpan.FromTicks(retryPolicy.InitialInterval.Ticks);
        }

        public RetryPolicy(int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null)
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
            InitialInterval = initialInterval ?? TimeSpan.FromMilliseconds(10);
        }

        public int MaxAttempts { get; }
        public TimeSpan Timeout { get; }
        public TimeSpan InitialInterval { get; }

        public virtual bool CanRetry(Exception exception)
        {
            return true;
        }
    }
}