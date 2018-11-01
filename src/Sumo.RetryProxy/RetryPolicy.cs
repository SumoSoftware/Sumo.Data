using System;
using System.Collections.Generic;

namespace Sumo.Retry
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

    public sealed class FilterRetryPolicy : RetryPolicy
    {
        public FilterRetryPolicy(RetryPolicy retryPolicy) : base(retryPolicy)
        {
        }

        public FilterRetryPolicy(int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null,
            IEnumerable<Type> exceptionWhiteList = null,
            IEnumerable<Type> exceptionBlackList = null)
            : base(maxAttempts, timeout, initialInterval)
        {
            ExceptionWhiteList = exceptionWhiteList != null ? new List<Type>(exceptionWhiteList) : new List<Type>();
            ExceptionBlackList = exceptionBlackList != null ? new List<Type>(exceptionBlackList) : new List<Type>();
        }

        public List<Type> ExceptionWhiteList { get; }
        public List<Type> ExceptionBlackList { get; }

        public override bool CanRetry(Exception exception)
        {
            var type = exception.GetType();
            return ExceptionWhiteList.Contains(type) && !ExceptionBlackList.Contains(type);
        }
    }

    public sealed class FunctionalRetryPolicy : RetryPolicy
    {
        public FunctionalRetryPolicy(RetryPolicy retryPolicy, Func<Exception, bool> isRetryAllowed) : base(retryPolicy)
        {
        }

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

    public abstract class CustomRetryPolicy : RetryPolicy, IRetryAllowedTester
    {
        public CustomRetryPolicy(RetryPolicy retryPolicy) : base(retryPolicy)
        {
        }

        public CustomRetryPolicy(int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null) : base(maxAttempts, timeout, initialInterval)
        {
        }

        public abstract bool IsRetryAllowed(Exception exception);

        public override bool CanRetry(Exception exception)
        {
            return IsRetryAllowed(exception);
        }
    }
}