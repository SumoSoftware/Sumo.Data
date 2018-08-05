using System;

namespace Sumo.Retry
{
    public interface ICanRetryTester
    {
        bool CanRetry(Exception exception);
    }
}
