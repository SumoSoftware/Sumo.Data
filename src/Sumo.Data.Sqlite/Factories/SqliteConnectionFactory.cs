using Microsoft.Data.Sqlite;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Factories.Sqlite
{
    //https://github.com/aspnet/Microsoft.Data.Sqlite/wiki/Connection-Strings
    //https://www.sqlite.org/uri.html
    public class SqliteConnectionFactory : IConnectionFactory
    {
        private readonly String _connectionString;
        public SqliteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqliteConnectionFactory()
        {

        }

        IParameterFactory _parameterFactory = new SqliteParameterFactory();
        public IParameterFactory ParameterFactory => _parameterFactory;

        IDataAdapterFactory _dataAdapterFactory = new SqliteDataAdapterFactory();
        public IDataAdapterFactory DataAdapterFactory => _dataAdapterFactory;

        public DbConnection Open(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            var connection = new SqliteConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch
            {
                connection.Close();
                connection.Dispose();
                throw;
            }
            return connection;
        }

        public async Task<DbConnection> OpenAsync(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            var connection = new SqliteConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch
            {
                connection.Close();
                connection.Dispose();
                throw;
            }
            return connection;
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
