namespace Sumo.Data.Schema.Sqlite
{
    public class SqliteSchemaParameterFactory : ISchemaParameterFactory
    {
        public string GetWriteParameterName<T>(int parameterIndex) where T : class
        {
            return SqliteEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        }
    }
}
