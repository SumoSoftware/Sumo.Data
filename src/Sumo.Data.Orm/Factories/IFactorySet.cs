using Sumo.Data.Schema;

namespace Sumo.Data.Orm
{
    public interface IFactorySet
    {
        IDataComponentFactory DataProviderFactory { get; }
        ISchemaParameterNames SchemaParameterFactory { get; }
        IScriptBuilder ScriptBuilder { get; }
        ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
