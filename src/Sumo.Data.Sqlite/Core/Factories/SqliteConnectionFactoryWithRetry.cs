using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Sqlite
{
    public class SqliteConnectionFactoryWithRetry : IConnectionFactory
    {
        private readonly IConnectionFactory _proxy;

        public SqliteConnectionFactoryWithRetry(SqliteTransientRetryPolicy retryPolicy) : this(retryPolicy, string.Empty) { }

        public SqliteConnectionFactoryWithRetry(SqliteTransientRetryPolicy retryPolicy, string connectionString) : base()
        {
            if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                string.IsNullOrEmpty(connectionString) ? new SqliteConnectionFactory() : new SqliteConnectionFactory(connectionString),
                retryPolicy);
        }

        public SqliteConnectionFactoryWithRetry(SqliteTransientRetryPolicy retryPolicy, IConnectionStringFactory connectionStringFactory)
        {
            if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                connectionStringFactory == null ? new SqliteConnectionFactory() : new SqliteConnectionFactory(connectionStringFactory),
                retryPolicy);
        }

        public SqliteConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new SqliteTransientRetryPolicy(maxAttempts, timeout))
        { }

        public SqliteConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout, string connectionString) :
            this(new SqliteTransientRetryPolicy(maxAttempts, timeout), connectionString)
        { }

        public SqliteConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout, IConnectionStringFactory connectionStringFactory) :
            this(new SqliteTransientRetryPolicy(maxAttempts, timeout), connectionStringFactory)
        { }

        public DbConnection Open(string connectionString)
        {
            return _proxy.Open(connectionString);
        }

        public Task<DbConnection> OpenAsync(string connectionString)
        {
            return _proxy.OpenAsync(connectionString);
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
