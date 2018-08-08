using Sumo.Retry;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public class SqlServerDataComponentFactory : DataComponentFactory
    {
        public SqlServerDataComponentFactory() : this(string.Empty) { }

        public SqlServerDataComponentFactory(string connectionString) : base()
        {
            _connectionFactory =
                string.IsNullOrEmpty(connectionString) ?
                new SqlServerConnectionFactory() :
                new SqlServerConnectionFactory(connectionString);

            _transactionFactory = new TransactionFactory();
            _dataAdapterFactory = new SqlServerDataAdapterFactory();
            _parameterFactory = new SqlServerParameterFactory();
        }

        public SqlServerDataComponentFactory(RetryOptions retryOptions) : this(retryOptions, string.Empty) { }

        public SqlServerDataComponentFactory(RetryOptions retryOptions, string connectionString) : base()
        {
            _connectionFactory =
                string.IsNullOrEmpty(connectionString) ?
                new SqlServerConnectionFactoryWithRetry(retryOptions) :
                new SqlServerConnectionFactoryWithRetry(retryOptions, connectionString);

            _transactionFactory = new SqlServerTransactionFactoryWithRetry(retryOptions);
            _dataAdapterFactory = new SqlServerDataAdapterFactory();
            _parameterFactory = new SqlServerParameterFactory();
        }

        private readonly IConnectionFactory _connectionFactory;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IDataAdapterFactory _dataAdapterFactory;
        private readonly IParameterFactory _parameterFactory;

        public override DbDataAdapter CreateDataAdapter(DbCommand dbCommand)
        {
            return _dataAdapterFactory.CreateDataAdapter(dbCommand);
        }

        public override DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size)
        {
            return _parameterFactory.CreateParameter(name, value, direction, size);
        }

        public override DbParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            return _parameterFactory.CreateParameter(name, value, direction);
        }

        public override DbParameter CreateReturnParameter(string name)
        {
            return _parameterFactory.CreateReturnParameter(name);
        }

        public override string GetParameterName(string name, int index)
        {
            return _parameterFactory.GetParameterName(name, index);
        }

        public override DbConnection Open(string connectionString)
        {
            return _connectionFactory.Open(connectionString);
        }

        public override DbConnection Open()
        {
            return _connectionFactory.Open();
        }

        public override Task<DbConnection> OpenAsync(string connectionString)
        {
            return _connectionFactory.OpenAsync(connectionString);
        }

        public override Task<DbConnection> OpenAsync()
        {
            return _connectionFactory.OpenAsync();
        }

        public override DbTransaction BeginTransaction(DbConnection dbConnection)
        {
            return _transactionFactory.BeginTransaction(dbConnection);
        }

        public override DbTransaction BeginTransaction(DbConnection dbConnection, IsolationLevel isolationLevel)
        {
            return _transactionFactory.BeginTransaction(dbConnection, isolationLevel);
        }
    }
}
