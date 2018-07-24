using Sumo.Data.Factories;
using Sumo.Data.Schema;
using System;

namespace Sumo.Data.Orm.Factories
{
    public class FactorySet : IFactorySet
    {
        public FactorySet(
            IConnectionFactory connectionFactory, 
            IDataAdapterFactory dataAdapterFactory, 
            IParameterFactory parameterFactory, 
            ITransactionFactory transactionFactory,
            IScriptBuilder scriptBuilder,
            ISqlStatementBuilder sqlStatementBuilder) : base()
        {
            ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            DataAdapterFactory = dataAdapterFactory ?? throw new ArgumentNullException(nameof(dataAdapterFactory));
            ParameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
            TransactionFactory = transactionFactory ?? throw new ArgumentNullException(nameof(transactionFactory));
            ScriptBuilder = scriptBuilder ?? throw new ArgumentNullException(nameof(scriptBuilder));
            SqlStatementBuilder = sqlStatementBuilder ?? throw new ArgumentNullException(nameof(sqlStatementBuilder));
        }

        public IConnectionFactory ConnectionFactory { get; }
        public IDataAdapterFactory DataAdapterFactory { get; }
        public IParameterFactory ParameterFactory { get; }
        public ITransactionFactory TransactionFactory { get; }
        public IScriptBuilder ScriptBuilder { get; }
        public ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
