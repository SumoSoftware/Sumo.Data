using System;

namespace Sumo.Data.Orm
{
    public class OrmException : Exception
    {
        public OrmException()
        {
        }

        public OrmException(string message) : base(message)
        {
        }

        public OrmException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class TableNotFoundException : OrmException
    {
        public TableNotFoundException(string tableName) : base()
        {
            TableName = tableName;
        }

        public TableNotFoundException(string tableName, string message) : base(message)
        {
            TableName = tableName;
        }

        public TableNotFoundException(string tableName, string message, Exception innerException) : base(message, innerException)
        {
            TableName = tableName;
        }

        public string TableName { get; }
    }

}
