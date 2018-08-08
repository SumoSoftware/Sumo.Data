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
    }
}
