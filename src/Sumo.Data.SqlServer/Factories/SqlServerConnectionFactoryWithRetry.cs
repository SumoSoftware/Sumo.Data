using Sumo.Data.Exceptions.SqlServer;
using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Factories.SqlServer
{
    public sealed class SqlServerConnectionFactoryWithRetry : IConnectionFactory
    {

        private string _connectionString;
        public SqlServerConnectionFactoryWithRetry(String connectionString)
        {
            _connectionString = connectionString;
        }


        private readonly IConnectionFactory _proxy;

        public SqlServerConnectionFactoryWithRetry(RetryOptions retryOptions)
        {
            if (retryOptions == null) throw new ArgumentNullException(nameof(retryOptions));

            _proxy = RetryProxy.Create<IConnectionFactory>(
                new SqlServerConnectionFactory(),
                retryOptions,
                new SqlServerTransientErrorTester());
        }

        public SqlServerConnectionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new RetryOptions(maxAttempts, timeout))
        { }



        IParameterFactory _paramFactory = new SqlServerParameterFactory();
        public IParameterFactory ParameterFactory => _paramFactory;


        IDataAdapterFactory _adapterFacotry = new SqlServerDataAdapterFactory();
        public IDataAdapterFactory DataAdapterFactory => _adapterFacotry;

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
