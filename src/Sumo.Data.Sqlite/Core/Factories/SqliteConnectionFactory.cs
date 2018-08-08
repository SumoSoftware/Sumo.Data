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

        public DbConnection Open(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

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
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

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
            if (String.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException($"Please construct {nameof(SqliteConnectionFactory)} with a connection string to use parameterless Open");
            return Open(_connectionString);
        }

        public Task<DbConnection> OpenAsync()
        {
            if (String.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException($"Please construct {nameof(SqliteConnectionFactory)} with a connection string to use parameterless OpenAsync");
            return OpenAsync(_connectionString);
        }
    }
}
