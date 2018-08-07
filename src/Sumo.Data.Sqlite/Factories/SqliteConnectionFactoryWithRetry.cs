using Sumo.Data.Exceptions.Sqlite;
using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Factories.Sqlite
{
    public class SqliteConnectionFactoryWithRetry : IConnectionFactory
    {
        private string _connectionString;
        public SqliteConnectionFactoryWithRetry(String connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly IConnectionFactory _proxy;

        IParameterFactory _parameterFactory = new SqliteParameterFactory();
        public IParameterFactory ParameterFactory => _parameterFactory;

        IDataAdapterFactory _dataAdapterFactory = new SqliteDataAdapterFactory();
        public IDataAdapterFactory DataAdapterFactory => _dataAdapterFactory;


        public SqliteConnectionFactoryWithRetry(RetryOptions retryOptions)
        {
            if (retryOptions == null) throw new ArgumentNullException(nameof(retryOptions));

            _proxy = RetryProxy.Create<IConnectionFactory>(
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

        public DbConnection Open()
        {
            if (String.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException("Please construct SqlServerConnectionFactory with a connection string to use parameterless Open");
            return Open(_connectionString);
        }

        public Task<DbConnection> OpenAsync()
        {
            if (String.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException("Please construct SqlServerConnectionFactory with a connection string to use parameterless OpenAsync");
            return OpenAsync(_connectionString);
        }
    }
}
