using System;
using System.Collections.Generic;

namespace Sumo.Retry
{
    public class RetryException : Exception, IRetryProxyException
    {
        public RetryException()
        {
        }

        public RetryException(string message) : base(message)
        {
        }

        public RetryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public int Attempts { get; internal set; }
        public TimeSpan Duration { get; internal set; }
        public List<Exception> Exceptions { get; internal set; }
    }

    public sealed class RetryNotAllowedException : RetryException
    {
        public RetryNotAllowedException()
        {
        }

        public RetryNotAllowedException(string message) : base(message)
        {
        }

        public RetryNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public sealed class ExceededMaxAttemptsException : RetryException
    {
        public ExceededMaxAttemptsException()
        {
        }

        public ExceededMaxAttemptsException(string message) : base(message)
        {
        }

        public ExceededMaxAttemptsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public sealed class ExceededMaxWaitTimeException : RetryException
    {
        public ExceededMaxWaitTimeException()
        {
        }

        public ExceededMaxWaitTimeException(string message) : base(message)
        {
        }

        public ExceededMaxWaitTimeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
