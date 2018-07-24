using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sumo.Data.Factories.SqlServer
{
    public sealed class SqlServerConnectionFactory : IConnectionFactory
    {
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
    }
}
