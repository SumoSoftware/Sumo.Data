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

        private RetryException CheckAttempts(RetrySession retrySession, Exception exception)
        {
            if (retrySession == null)
            {
                throw new ArgumentNullException(nameof(retrySession));
            }
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return ++retrySession.Attempts >= MaxAttempts
                ? new ExceededMaxAttemptsException(retrySession, this, exception)
                : null;
        }

        private RetryException CheckTimeout(RetrySession retrySession, Exception exception)
        {
            if (retrySession == null)
            {
                throw new ArgumentNullException(nameof(retrySession));
            }
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (!retrySession.Active)
            {
                retrySession.Begin();
            }

            return retrySession.ElapsedTime >= Timeout
                ? new ExceededMaxWaitTimeException(retrySession, this, exception)
                : null;
        }

        public virtual RetryException Check(RetrySession retrySession, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            if (retrySession == null)
            {
                throw new ArgumentNullException(nameof(retrySession));
            }

            retrySession.Exceptions.Add(exception);

            var result = CheckAttempts(retrySession, exception);
            if (result == null)
            {
                result = CheckTimeout(retrySession, exception);
            }
            return result;
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

        public override RetryException Check(RetrySession retrySession, Exception exception)
        {
            var result = base.Check(retrySession, exception);
            if (result == null)
            {
                var type = exception.GetType();
                if (!ExceptionWhiteList.Contains(type))
                {
                    result = new RetryNotAllowedException(retrySession, this, exception);
                }
                if (result == null && ExceptionBlackList.Contains(type))
                {
                    result = new RetryNotAllowedException(retrySession, this, exception);
                }
            }
            return result;
        }
    }

    public sealed class FunctionalRetryPolicy : RetryPolicy
    {
        public FunctionalRetryPolicy(RetryPolicy retryPolicy) : base(retryPolicy)
        {
        }

        public FunctionalRetryPolicy(Func<Exception, bool> isRetryAllowed, int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null)
            : base(maxAttempts, timeout, initialInterval)
        {
            IsRetryAllowed = isRetryAllowed ?? throw new ArgumentNullException(nameof(isRetryAllowed));
        }

        public Func<Exception, bool> IsRetryAllowed { get; }

        public override RetryException Check(RetrySession retrySession, Exception exception)
        {
            var result = base.Check(retrySession, exception);
            if (result == null && !IsRetryAllowed(exception))
            {
                result = new RetryNotAllowedException(retrySession, this, exception);
            }
            return result;
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

        public override RetryException Check(RetrySession retrySession, Exception exception)
        {
            var result = base.Check(retrySession, exception);
            if (result == null && !IsRetryAllowed(exception))
            {
                result = new RetryNotAllowedException(retrySession, this, exception);
            }
            return result;
        }
    }
}