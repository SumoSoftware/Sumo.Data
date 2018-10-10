using System;

namespace Sumo.Retry
{
    public class RetryException : Exception
    {
        public RetryException(RetrySession retrySession) : base()
        {
            RetrySession = retrySession ?? throw new ArgumentNullException(nameof(retrySession));
        }

        public RetryException(RetrySession retrySession, string message) : base(message)
        {
            RetrySession = retrySession ?? throw new ArgumentNullException(nameof(retrySession));
        }

        public RetryException(RetrySession retrySession, string message, Exception innerException) : base(message, innerException)
        {
            RetrySession = retrySession ?? throw new ArgumentNullException(nameof(retrySession));
        }

        public RetryException(RetrySession retrySession, Exception innerException) : this(retrySession, string.Empty, innerException)
        {
        }

        public RetrySession RetrySession { get; }
    }

    public class RetryNotAllowedException : RetryException
    {
        public RetryNotAllowedException(RetrySession retrySession, RetryPolicy retryPolicy) : base(retrySession)
        {
            RetryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        }

        public RetryNotAllowedException(RetrySession retrySession, RetryPolicy retryPolicy, Exception innerException) : base(retrySession, innerException)
        {
            RetryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        }

        public RetryPolicy RetryPolicy { get; }
    }

    public sealed class ExceededMaxAttemptsException : RetryNotAllowedException
    {
        public ExceededMaxAttemptsException(RetrySession retrySession, RetryPolicy retryPolicy) : base(retrySession, retryPolicy)
        {
        }

        public ExceededMaxAttemptsException(RetrySession retrySession, RetryPolicy retryPolicy, Exception innerException) : base(retrySession, retryPolicy, innerException)
        {
        }
    }

    public sealed class ExceededMaxWaitTimeException : RetryNotAllowedException
    {
        public ExceededMaxWaitTimeException(RetrySession retrySession, RetryPolicy retryPolicy) : base(retrySession, retryPolicy)
        {
        }

        public ExceededMaxWaitTimeException(RetrySession retrySession, RetryPolicy retryPolicy, Exception innerException) : base(retrySession, retryPolicy, innerException)
        {
        }
    }
}
