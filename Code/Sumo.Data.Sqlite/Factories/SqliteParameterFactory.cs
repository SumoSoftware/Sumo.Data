using Microsoft.Data.Sqlite;
using Sumo.Data.Names.Sqlite;
using Sumo.Data.Types.Sqlite;
using System.Data;
using System.Data.Common;

namespace Sumo.Data.Factories.Sqlite
{
    public sealed class SqliteParameterFactory : IParameterFactory
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

        public string GetParameterName(string name)
        {
            return new SqliteParameterName(name).ToString();
        }

        public string GetWriteParameterName<T>(int parameterIndex) where T : class
        {
            return SqliteEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        }
    }
}
