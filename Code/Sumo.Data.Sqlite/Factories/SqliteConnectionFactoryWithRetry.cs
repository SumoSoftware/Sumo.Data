using Sumo.Data.Exceptions.Sqlite;
using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Factories.Sqlite
{
    public class SqliteConnectionFactoryWithRetry : IConnectionFactory
    {
        private readonly IConnectionFactory _proxy;

        public SqliteConnectionFactoryWithRetry(RetryOptions retryOptions)
        {
            if (retryOptions == null) throw new ArgumentNullException(nameof(retryOptions));

            _proxy = Retry.Create<IConnectionFactory>(
                new SqliteConnectionFactory(),
                retryOptions,
                new SqliteTransientErrorTester());
        }

        public SqliteConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new RetryOptions(maxAttempts, timeout))
        { }

        public DbConnection Open(string connectionString)
        {
            return _proxy.Open(connectionString);
        }

        public async Task<DbConnection> OpenAsync(string connectionString)
        {
            return await _proxy.OpenAsync(connectionString);
        }
    }
}
