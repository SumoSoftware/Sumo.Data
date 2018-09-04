using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerConnectionFactory : IConnectionFactory
    {
        public SqlServerConnectionFactory() { }

        private readonly string _connectionString;
        public SqlServerConnectionFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
        }

        private readonly IConnectionStringFactory _connectionStringFactory;
        public SqlServerConnectionFactory(IConnectionStringFactory connectionStringFactory) 
        {
            _connectionStringFactory = connectionStringFactory ?? throw new ArgumentNullException(nameof(connectionStringFactory));
        }

        public DbConnection Open(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                connection.Close();
                connection.Dispose();
                if (ex.Number == 10054) SqlConnection.ClearPool(connection);
                throw;
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

            var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch (SqlException ex)
            {
                connection.Close();
                connection.Dispose();
                if (ex.Number == 10054) SqlConnection.ClearPool(connection);
                throw;
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
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"Please construct {nameof(SqlServerConnectionFactory)} with a connection string or factory to use parameterless Open");

            return Open(connectionString);
        }

        public async Task<DbConnection> OpenAsync()
        {
            var connectionString = string.IsNullOrEmpty(_connectionString) ? await _connectionStringFactory.GetConnectionStringAsync() : _connectionString;
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"Please construct {nameof(SqlServerConnectionFactory)} with a connection string or factory to use parameterless Open");

            return await OpenAsync(_connectionString);
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
