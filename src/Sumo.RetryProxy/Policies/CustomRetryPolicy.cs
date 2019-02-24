using System;

namespace Sumo.Retry.Policies
{
    public abstract class CustomRetryPolicy : RetryPolicy, IRetryAllowedTester
    {
        public CustomRetryPolicy(RetryPolicy retryPolicy) : base(retryPolicy) { }

        public CustomRetryPolicy(int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null)
            : base(maxAttempts, timeout, initialInterval) { }

        public abstract bool IsRetryAllowed(Exception exception);

        public override bool CanRetry(Exception exception)
        {
            return IsRetryAllowed(exception);
        }
    }
}
