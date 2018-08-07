using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sumo.Data.Factories.SqlServer
{
    public sealed class SqlServerConnectionFactory : IConnectionFactory
    {
        private string _connectionString;
        public SqlServerConnectionFactory(String connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlServerConnectionFactory()
        {

        }


        IParameterFactory _paramFactory = new SqlServerParameterFactory();
        public IParameterFactory ParameterFactory => _paramFactory;


        IDataAdapterFactory _adapterFacotry = new SqlServerDataAdapterFactory();
        public IDataAdapterFactory DataAdapterFactory => _adapterFacotry;


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
            if(String.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException("Please construct SqlServerConnectionFactory with a connection string to use parameterless Open");
            return Open(_connectionString);
        }

        public Task<DbConnection> OpenAsync()
        {
            if (String.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException("Please construct SqlServerConnectionFactory with a connection string to use parameterless OpenAsync");
            return OpenAsync(_connectionString);
        }
    }
}
