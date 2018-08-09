using System;

namespace Sumo.Data.Retry
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
