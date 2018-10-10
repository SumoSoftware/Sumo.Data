using System;

namespace Sumo.Retry
{
    public interface IRetryAllowedTester
    {
        bool IsRetryAllowed(Exception exception);
    }
}
