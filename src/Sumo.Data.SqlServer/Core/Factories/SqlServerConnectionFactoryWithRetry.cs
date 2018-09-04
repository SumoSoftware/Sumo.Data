using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerConnectionFactoryWithRetry : IConnectionFactory
    {
        private readonly IConnectionFactory _proxy;

        public SqlServerConnectionFactoryWithRetry(RetryOptions retryOptions) : this(retryOptions, string.Empty) { }

        public SqlServerConnectionFactoryWithRetry(RetryOptions retryOptions, string connectionString)
        {
            if (retryOptions == null) throw new ArgumentNullException(nameof(retryOptions));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                string.IsNullOrEmpty(connectionString) ? new SqlServerConnectionFactory() : new SqlServerConnectionFactory(connectionString),
                retryOptions,
                new SqlServerTransientErrorTester());
        }

        public SqlServerConnectionFactoryWithRetry(RetryOptions retryOptions, IConnectionStringFactory connectionStringFactory)
        {
            if (retryOptions == null) throw new ArgumentNullException(nameof(retryOptions));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                connectionStringFactory == null ? new SqlServerConnectionFactory() : new SqlServerConnectionFactory(connectionStringFactory),
                retryOptions,
                new SqlServerTransientErrorTester());
        }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new RetryOptions(maxAttempts, timeout))
        { }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout, string connectionString) :
            this(new RetryOptions(maxAttempts, timeout), connectionString)
        { }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout, IConnectionStringFactory connectionStringFactory) :
            this(new RetryOptions(maxAttempts, timeout), connectionStringFactory)
        { }

        public DbConnection Open(string connectionString)
        {
            return _proxy.Open(connectionString);
        }

        public async Task<DbConnection> OpenAsync(string connectionString)
        {
            return await _proxy.OpenAsync(connectionString);
        }

        public DbConnection Open()
        {
            return _proxy.Open();
        }

        public Task<DbConnection> OpenAsync()
        {
            return _proxy.OpenAsync();
        }

        public DbConnection Open(IConnectionStringFactory connectionStringFactory)
        {
            return _proxy.Open(connectionStringFactory);
        }

        public Task<DbConnection> OpenAsync(IConnectionStringFactory connectionStringFactory)
        {
            return _proxy.OpenAsync(connectionStringFactory);
        }
    }
}
