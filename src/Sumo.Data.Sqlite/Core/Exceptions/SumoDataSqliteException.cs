using System;

namespace Sumo.Data.Sqlite
{
    public class SumoDataSqliteException : SumoDataException
    {
        public SumoDataSqliteException()
        {
        }

        public SumoDataSqliteException(string message) : base(message)
        {
        }

        public SumoDataSqliteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
