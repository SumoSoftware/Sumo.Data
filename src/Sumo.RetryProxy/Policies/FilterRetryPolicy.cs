using System;
using System.Collections.Generic;

namespace Sumo.Retry.Policies
{
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

}
