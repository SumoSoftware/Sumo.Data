using Sumo.Data.Factories.SqlServer;
using Sumo.Data.Schema.Types.SqlServer;

namespace Sumo.Data.Schema.Factories.SqlServer
{
    public class SqlServerSchemaParameterFactory: SqlServerParameterFactory, ISchemaParameterFactory
    {
        public string GetWriteParameterName<T>(int parameterIndex) where T : class
        {
            return SqlServerEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        }
    }
}
