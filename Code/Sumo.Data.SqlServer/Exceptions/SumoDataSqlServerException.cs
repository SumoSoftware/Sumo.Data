using System;

namespace Sumo.Data.Exceptions.SqlServer
{
    public class SumoDataSqlServerException : SumoDataException
    {
        public SumoDataSqlServerException()
        {
        }

        public SumoDataSqlServerException(string message) : base(message)
        {
        }

        public SumoDataSqlServerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
