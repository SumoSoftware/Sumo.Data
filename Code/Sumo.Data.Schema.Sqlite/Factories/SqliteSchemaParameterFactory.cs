using Sumo.Data.Factories.Sqlite;
using Sumo.Data.Schema.Sqlite.Types;

namespace Sumo.Data.Schema.Factories.Sqlite
{
    public class SqliteSchemaParameterFactory : SqliteParameterFactory, ISchemaParameterFactory
    {
        public string GetWriteParameterName<T>(int parameterIndex) where T : class
        {
            return SqliteEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        }
    }
}
