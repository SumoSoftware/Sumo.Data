using Sumo.Data.Schema;

namespace Sumo.Data.Orm
{
    public interface IFactorySet
    {
        IDataProviderFactory DataProviderFactory { get; }
        ISchemaParameterFactory SchemaParameterFactory { get; }
        IScriptBuilder ScriptBuilder { get; }
        ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
