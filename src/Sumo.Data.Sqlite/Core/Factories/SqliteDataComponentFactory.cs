using Sumo.Retry;
using System;

namespace Sumo.Data.Sqlite
{
    public class SqliteDataComponentFactory : DataComponentFactory
    {
        public SqliteDataComponentFactory() : this(String.Empty) { }

        public SqliteDataComponentFactory(string connectionString) : base()
        {
            _connectionFactory =
                String.IsNullOrEmpty(connectionString) ?
                new SqliteConnectionFactory() :
                new SqliteConnectionFactory(connectionString);

            _transactionFactory = new TransactionFactory();
            _dataAdapterFactory = new SqliteDataAdapterFactory();
            _parameterFactory = new SqliteParameterFactory();
        }

        public SqliteDataComponentFactory(RetryOptions retryOptions) : this(retryOptions, String.Empty) { }

        public SqliteDataComponentFactory(RetryOptions retryOptions, string connectionString) : base()
        {
            _connectionFactory =
                String.IsNullOrEmpty(connectionString) ?
                new SqliteConnectionFactoryWithRetry(retryOptions) :
                new SqliteConnectionFactoryWithRetry(retryOptions, connectionString);

            _transactionFactory = new SqliteTransactionFactoryWithRetry(retryOptions);
            _dataAdapterFactory = new SqliteDataAdapterFactory();
            _parameterFactory = new SqliteParameterFactory();
        }
    }
}
