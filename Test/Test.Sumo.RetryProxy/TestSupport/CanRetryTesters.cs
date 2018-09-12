using System;

namespace Sumo.Retry
{
    public class CanRetryProxySubjectException : IExceptionWhiteList
    {
        public bool CanRetry(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            return exception is ProxySubjectException;
        }
    }

    public class CanNotRetryProxySubjectException : IExceptionWhiteList
    {
        public bool CanRetry(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            return !(exception is ProxySubjectException);
        }
    }
}
