using Sumo.Data.Factories;
using Sumo.Data.Schema;

namespace Sumo.Data.Orm.Factories
{
    public interface IFactorySet
    {
        IConnectionFactory ConnectionFactory { get; }
        IDataAdapterFactory DataAdapterFactory { get; }
        IParameterFactory ParameterFactory { get; }
        ITransactionFactory TransactionFactory { get; }
        IScriptBuilder ScriptBuilder { get; }
        ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
