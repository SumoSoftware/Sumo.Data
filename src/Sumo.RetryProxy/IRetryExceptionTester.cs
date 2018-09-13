using System;

namespace Sumo.Retry
{
    public interface IRetryExceptionTester
    {
        bool CanRetry(Exception exception);
    }
}
