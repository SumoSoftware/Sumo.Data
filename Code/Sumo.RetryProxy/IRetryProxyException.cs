using System;
using System.Collections.Generic;

namespace Sumo.Retry
{
    public interface IRetryProxyException
    {
        int Attempts { get; }
        TimeSpan Duration { get; }
        List<Exception> Exceptions { get; }
    }
}
