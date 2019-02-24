using System;

namespace Sumo.Retry.Policies
{
    public sealed class FunctionalRetryPolicy : RetryPolicy
    {
        public FunctionalRetryPolicy(RetryPolicy retryPolicy, Func<Exception, bool> isRetryAllowed) : base(retryPolicy) { }

        public FunctionalRetryPolicy(Func<Exception, bool> isRetryAllowed, int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null)
            : base(maxAttempts, timeout, initialInterval)
        {
            IsRetryAllowed = isRetryAllowed ?? throw new ArgumentNullException(nameof(isRetryAllowed));
        }

        public Func<Exception, bool> IsRetryAllowed { get; }

        public override bool CanRetry(Exception exception)
        {
            return IsRetryAllowed(exception);
        }
    }

}
