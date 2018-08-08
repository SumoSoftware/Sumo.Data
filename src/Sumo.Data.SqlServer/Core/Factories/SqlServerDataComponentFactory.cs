using Sumo.Retry;
using System;

namespace Sumo.Data.SqlServer
{
    public class SqlServerDataComponentFactory : DataComponentFactory
    {
        public SqlServerDataComponentFactory() : this(String.Empty) { }

        public SqlServerDataComponentFactory(string connectionString) : base()
        {
            _connectionFactory =
                String.IsNullOrEmpty(connectionString) ?
                new SqlServerConnectionFactory() :
                new SqlServerConnectionFactory(connectionString);

            _transactionFactory = new TransactionFactory();
            _dataAdapterFactory = new SqlServerDataAdapterFactory();
            _parameterFactory = new SqlServerParameterFactory();
        }

        public SqlServerDataComponentFactory(RetryOptions retryOptions) : this(retryOptions, String.Empty) { }

        public SqlServerDataComponentFactory(RetryOptions retryOptions, string connectionString) : base()
        {
            _connectionFactory =
                String.IsNullOrEmpty(connectionString) ?
                new SqlServerConnectionFactoryWithRetry(retryOptions) :
                new SqlServerConnectionFactoryWithRetry(retryOptions, connectionString);

            _transactionFactory = new SqlServerTransactionFactoryWithRetry(retryOptions);
            _dataAdapterFactory = new SqlServerDataAdapterFactory();
            _parameterFactory = new SqlServerParameterFactory();
        }
    }
}
