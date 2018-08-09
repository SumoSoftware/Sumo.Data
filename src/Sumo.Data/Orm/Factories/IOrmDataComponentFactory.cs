using Sumo.Data.Schema;

namespace Sumo.Data.Orm
{
    public interface IOrmDataComponentFactory : IDataComponentFactory
    {
        ISchemaParameterNames SchemaParameterNames { get; }
        IScriptBuilder ScriptBuilder { get; }
        ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
