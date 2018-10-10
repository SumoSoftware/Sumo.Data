using Sumo.Retry;

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

        public SqlServerDataComponentFactory(SqlServerTransientRetryPolicy retryPolicy) : this(retryPolicy, string.Empty) { }

        public SqlServerDataComponentFactory(SqlServerTransientRetryPolicy retryPolicy, string connectionString) : base()
        {
            _connectionFactory =
                string.IsNullOrEmpty(connectionString) ?
                new SqlServerConnectionFactoryWithRetry(retryPolicy) :
                new SqlServerConnectionFactoryWithRetry(retryPolicy, connectionString);

            _transactionFactory = new SqlServerTransactionFactoryWithRetry(retryPolicy);
            _dataAdapterFactory = new SqlServerDataAdapterFactory();
            _parameterFactory = new SqlServerParameterFactory();
        }

        public SqlServerDataComponentFactory(IConnectionStringFactory connectionStringFactory) : base()
        {
            _connectionFactory =
                connectionStringFactory == null ?
                new SqlServerConnectionFactory() :
                new SqlServerConnectionFactory(connectionStringFactory);

            _transactionFactory = new TransactionFactory();
            _dataAdapterFactory = new SqlServerDataAdapterFactory();
            _parameterFactory = new SqlServerParameterFactory();
        }

        public SqlServerDataComponentFactory(SqlServerTransientRetryPolicy retryPolicy, IConnectionStringFactory connectionStringFactory) : base()
        {
            _connectionFactory =
                connectionStringFactory == null ?
                new SqlServerConnectionFactoryWithRetry(retryPolicy) :
                new SqlServerConnectionFactoryWithRetry(retryPolicy, connectionStringFactory);

            _transactionFactory = new SqlServerTransactionFactoryWithRetry(retryPolicy);
            _dataAdapterFactory = new SqlServerDataAdapterFactory();
            _parameterFactory = new SqlServerParameterFactory();
        }
    }
}
