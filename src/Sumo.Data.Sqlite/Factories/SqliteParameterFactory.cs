using Microsoft.Data.Sqlite;
using Sumo.Data.Names;
using System.Data;
using System.Data.Common;

namespace Sumo.Data.Factories.Sqlite
{
    public class SqliteParameterFactory : IParameterFactory
    {
        public DbParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            return new SqliteParameter(name, value) { Direction = direction };
        }

        public DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size)
        {
            return new SqliteParameter(name, value) { Direction = direction, Size = size };
        }

        public DbParameter CreateReturnParameter(string name)
        {
            return new SqliteParameter(name, SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue };
        }

        public string GetParameterName(string name, int index)
        {
            return new ParameterName(name, index).ToString();
        }
    }
}
