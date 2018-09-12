using System;

namespace Sumo.Retry
{
    public interface IExceptionWhiteList
    {
        bool CanRetry(Exception exception);
    }
}
