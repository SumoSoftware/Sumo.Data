namespace Sumo.Data.Schema.Sqlite
{
    public class SqliteSchemaParameterNames : ISchemaParameterNames
    {
        public string GetWriteParameterName<T>(int parameterIndex) where T : class
        {
            return SqliteEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        }
    }
}
