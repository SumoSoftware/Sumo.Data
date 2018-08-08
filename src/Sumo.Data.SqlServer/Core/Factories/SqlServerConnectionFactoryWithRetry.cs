using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerConnectionFactoryWithRetry : IConnectionFactory
    {
        private readonly IConnectionFactory _proxy;

        public SqlServerConnectionFactoryWithRetry(RetryOptions retryOptions) : this(retryOptions, null) { }

        public SqlServerConnectionFactoryWithRetry(RetryOptions retryOptions, string connectionString = null)
        {
            if (retryOptions == null) throw new ArgumentNullException(nameof(retryOptions));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                String.IsNullOrEmpty(connectionString) ? new SqlServerConnectionFactory() : new SqlServerConnectionFactory(connectionString),
                retryOptions,
                new SqlServerTransientErrorTester());
        }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new RetryOptions(maxAttempts, timeout))
        { }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout, string connectionString) :
            this(new RetryOptions(maxAttempts, timeout), connectionString)
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
    }
}
