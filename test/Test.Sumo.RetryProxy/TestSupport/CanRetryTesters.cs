using System;

namespace Sumo.Retry
{
    public class CanRetryProxySubjectPolicy : CustomRetryPolicy
    {
        public CanRetryProxySubjectPolicy(RetryPolicy retryPolicy) : base(retryPolicy)
        {
        }

        public CanRetryProxySubjectPolicy(int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null) : base(maxAttempts, timeout, initialInterval)
        {
        }

        public override bool IsRetryAllowed(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return exception is ProxySubjectTestException;
        }
    }

    public class CanNotRetryProxySubjectPolicy : CustomRetryPolicy
    {
        public CanNotRetryProxySubjectPolicy(RetryPolicy retryPolicy) : base(retryPolicy)
        {
        }

        public CanNotRetryProxySubjectPolicy(int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null) : base(maxAttempts, timeout, initialInterval)
        {
        }

        public override bool IsRetryAllowed(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return !(exception is ProxySubjectTestException);
        }
    }
}
