namespace Sumo.Data.Schema.SqlServer
{
    public class SqlServerSchemaParameterFactory: ISchemaParameterFactory
    {
        public string GetWriteParameterName<T>(int parameterIndex) where T : class
        {
            return SqlServerEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        }
    }
}
