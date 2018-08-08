namespace Sumo.Data.Schema.SqlServer
{
    public class SqlServerSchemaParameterNames: ISchemaParameterNames
    {
        public string GetWriteParameterName<T>(int parameterIndex) where T : class
        {
            return SqlServerEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        }
    }
}
