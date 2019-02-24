using System;

namespace Sumo.Retry.Policies
{
    public interface IRetryAllowedTester
    {
        bool IsRetryAllowed(Exception exception);
    }
}
