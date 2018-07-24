using System;

namespace Sumo.Data.Retry.TestSupport
{
    internal class ProxySubjectException : Exception
    {
        public ProxySubjectException()
        {
        }

        public ProxySubjectException(string message) : base(message)
        {
        }

        public ProxySubjectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
