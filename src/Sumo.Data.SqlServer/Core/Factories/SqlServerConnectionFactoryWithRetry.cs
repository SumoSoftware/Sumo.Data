using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerConnectionFactoryWithRetry : IConnectionFactory
    {
        private readonly IConnectionFactory _proxy;

        public SqlServerConnectionFactoryWithRetry(SqlServerTransientRetryPolicy retryPolicy) : this(retryPolicy, string.Empty) { }

        public SqlServerConnectionFactoryWithRetry(SqlServerTransientRetryPolicy retryPolicy, string connectionString)
        {
            if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                string.IsNullOrEmpty(connectionString) ? new SqlServerConnectionFactory() : new SqlServerConnectionFactory(connectionString),
                retryPolicy);
        }

        public SqlServerConnectionFactoryWithRetry(SqlServerTransientRetryPolicy retryPolicy, IConnectionStringFactory connectionStringFactory)
        {
            if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                connectionStringFactory == null ? new SqlServerConnectionFactory() : new SqlServerConnectionFactory(connectionStringFactory),
                retryPolicy);
        }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new SqlServerTransientRetryPolicy(maxAttempts, timeout))
        { }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout, string connectionString) :
            this(new SqlServerTransientRetryPolicy(maxAttempts, timeout), connectionString)
        { }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout, IConnectionStringFactory connectionStringFactory) :
            this(new SqlServerTransientRetryPolicy(maxAttempts, timeout), connectionStringFactory)
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
