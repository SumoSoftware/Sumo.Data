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
    }
}
