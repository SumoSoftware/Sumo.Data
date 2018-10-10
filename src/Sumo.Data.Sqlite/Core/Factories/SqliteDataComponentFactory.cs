using Sumo.Retry;

namespace Sumo.Data.Sqlite
{
    public class SqliteDataComponentFactory : DataComponentFactory
    {
        public SqliteDataComponentFactory() : this(string.Empty) { }

        public SqliteDataComponentFactory(string connectionString) : base()
        {
            _connectionFactory =
                string.IsNullOrEmpty(connectionString) ?
                new SqliteConnectionFactory() :
                new SqliteConnectionFactory(connectionString);

            _transactionFactory = new TransactionFactory();
            _dataAdapterFactory = new SqliteDataAdapterFactory();
            _parameterFactory = new SqliteParameterFactory();
        }

        public SqliteDataComponentFactory(SqliteTransientRetryPolicy retryPolicy) : this(retryPolicy, string.Empty) { }

        public SqliteDataComponentFactory(SqliteTransientRetryPolicy retryPolicy, string connectionString) : base()
        {
            _connectionFactory =
                string.IsNullOrEmpty(connectionString) ?
                new SqliteConnectionFactoryWithRetry(retryPolicy) :
                new SqliteConnectionFactoryWithRetry(retryPolicy, connectionString);

            _transactionFactory = new SqliteTransactionFactoryWithRetry(retryPolicy);
            _dataAdapterFactory = new SqliteDataAdapterFactory();
            _parameterFactory = new SqliteParameterFactory();
        }

        public SqliteDataComponentFactory(IConnectionStringFactory connectionStringFactory) : base()
        {
            _connectionFactory =
                connectionStringFactory == null ?
                new SqliteConnectionFactory() :
                new SqliteConnectionFactory(connectionStringFactory);

            _transactionFactory = new TransactionFactory();
            _dataAdapterFactory = new SqliteDataAdapterFactory();
            _parameterFactory = new SqliteParameterFactory();
        }

        public SqliteDataComponentFactory(SqliteTransientRetryPolicy retryPolicy, IConnectionStringFactory connectionStringFactory) : base()
        {
            _connectionFactory =
                connectionStringFactory == null ?
                new SqliteConnectionFactoryWithRetry(retryPolicy) :
                new SqliteConnectionFactoryWithRetry(retryPolicy, connectionStringFactory);

            _transactionFactory = new SqliteTransactionFactoryWithRetry(retryPolicy);
            _dataAdapterFactory = new SqliteDataAdapterFactory();
            _parameterFactory = new SqliteParameterFactory();
        }
    }
}
