using System;
using System.Reflection;

namespace Sumo.Retry
{
    internal static class ExceptionExtensions
    {
        internal static Exception GetPrimaryException(this TargetInvocationException ex)
        {
            return ex.InnerException;
        }

        internal static Exception GetPrimaryException(this AggregateException ex)
        {
            return ex.GetBaseException();
        }

        internal static Exception GetPrimaryException(this Exception ex)
        {
            return ex;
        }        
    }
}
