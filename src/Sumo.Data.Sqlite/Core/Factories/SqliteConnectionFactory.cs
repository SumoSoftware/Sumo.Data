using Microsoft.Data.Sqlite;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Sqlite
{
    //https://github.com/aspnet/Microsoft.Data.Sqlite/wiki/Connection-Strings
    //https://www.sqlite.org/uri.html
    public class SqliteConnectionFactory : IConnectionFactory
    {
        public SqliteConnectionFactory() { }

        private readonly string _connectionString;
        public SqliteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly IConnectionStringFactory _connectionStringFactory;
        public SqliteConnectionFactory(IConnectionStringFactory connectionStringFactory)
        {
            _connectionStringFactory = connectionStringFactory ?? throw new ArgumentNullException(nameof(connectionStringFactory));
        }

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
            var connectionString = string.IsNullOrEmpty(_connectionString) ? _connectionStringFactory.GetConnectionString() : _connectionString;
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"Please construct {nameof(SqliteConnectionFactory)} with a connection string or factory to use parameterless Open");

            return Open(connectionString);
        }

        public async Task<DbConnection> OpenAsync()
        {
            var connectionString = string.IsNullOrEmpty(_connectionString) ? await _connectionStringFactory.GetConnectionStringAsync() : _connectionString;
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"Please construct {nameof(SqliteConnectionFactory)} with a connection string or factory to use parameterless Open");

            return await OpenAsync(connectionString);
        }

        public DbConnection Open(IConnectionStringFactory connectionStringFactory)
        {
            if (connectionStringFactory == null) throw new ArgumentNullException(nameof(connectionStringFactory));
            var connectionString = _connectionStringFactory.GetConnectionString();

            return Open(connectionString);
        }

        public async Task<DbConnection> OpenAsync(IConnectionStringFactory connectionStringFactory)
        {
            if (connectionStringFactory == null) throw new ArgumentNullException(nameof(connectionStringFactory));
            var connectionString = await _connectionStringFactory.GetConnectionStringAsync();

            return await OpenAsync(connectionString);
        }
    }
}
