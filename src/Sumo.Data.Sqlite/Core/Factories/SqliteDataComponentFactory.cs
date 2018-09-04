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

        public SqliteDataComponentFactory(RetryOptions retryOptions) : this(retryOptions, string.Empty) { }

        public SqliteDataComponentFactory(RetryOptions retryOptions, string connectionString) : base()
        {
            _connectionFactory =
                string.IsNullOrEmpty(connectionString) ?
                new SqliteConnectionFactoryWithRetry(retryOptions) :
                new SqliteConnectionFactoryWithRetry(retryOptions, connectionString);

            _transactionFactory = new SqliteTransactionFactoryWithRetry(retryOptions);
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

        public SqliteDataComponentFactory(RetryOptions retryOptions, IConnectionStringFactory connectionStringFactory) : base()
        {
            _connectionFactory =
                connectionStringFactory == null ?
                new SqliteConnectionFactoryWithRetry(retryOptions) :
                new SqliteConnectionFactoryWithRetry(retryOptions, connectionStringFactory);

            _transactionFactory = new SqliteTransactionFactoryWithRetry(retryOptions);
            _dataAdapterFactory = new SqliteDataAdapterFactory();
            _parameterFactory = new SqliteParameterFactory();
        }
    }
}
