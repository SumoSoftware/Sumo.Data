using Sumo.Data.Schema;

namespace Sumo.Data.Orm
{
    public interface IFactorySet
    {
        IDataComponentFactory DataProviderFactory { get; }
        ISchemaParameterFactory SchemaParameterFactory { get; }
        IScriptBuilder ScriptBuilder { get; }
        ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
