using System;

namespace Sumo.Retry
{
    internal class ProxySubjectTestException : Exception
    {
        public ProxySubjectTestException()
        {
        }

        public ProxySubjectTestException(string message) : base(message)
        {
        }

        public ProxySubjectTestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
