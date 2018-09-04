using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public abstract class DataComponentFactory : IDataComponentFactory
    {
        protected IConnectionFactory _connectionFactory;
        protected ITransactionFactory _transactionFactory;
        protected IDataAdapterFactory _dataAdapterFactory;
        protected IParameterFactory _parameterFactory;

        public DbDataAdapter CreateDataAdapter(DbCommand dbCommand)
        {
            return _dataAdapterFactory.CreateDataAdapter(dbCommand);
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction, int size)
        {
            return _parameterFactory.CreateParameter(name, type, direction, size);
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction)
        {
            return _parameterFactory.CreateParameter(name, type, direction);
        }

        public DbParameter CreateParameter(string name, DbType type)
        {
            return _parameterFactory.CreateParameter(name, type);
        }

        public DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size)
        {
            return _parameterFactory.CreateParameter(name, value, direction, size);
        }

        public DbParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            return _parameterFactory.CreateParameter(name, value, direction);
        }

        public DbParameter CreateParameter(string name, PropertyInfo property)
        {
            return _parameterFactory.CreateParameter(name, property);
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction)
        {
            return _parameterFactory.CreateParameter(name, property, direction);
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction, int size)
        {
            return _parameterFactory.CreateParameter(name, property, direction, size);
        }

        public DbParameter CreateReturnParameter(string name)
        {
            return _parameterFactory.CreateReturnParameter(name);
        }

        public string GetParameterName(string name, int index)
        {
            return _parameterFactory.GetParameterName(name, index);
        }

        public DbConnection Open()
        {
            return _connectionFactory.Open();
        }

        public DbConnection Open(IConnectionStringFactory connectionStringFactory)
        {
            return _connectionFactory.Open(connectionStringFactory);
        }

        public DbConnection Open(string connectionString)
        {
            return _connectionFactory.Open(connectionString);
        }

        public Task<DbConnection> OpenAsync()
        {
            return _connectionFactory.OpenAsync();
        }

        public Task<DbConnection> OpenAsync(string connectionString)
        {
            return _connectionFactory.OpenAsync(connectionString);
        }

        public Task<DbConnection> OpenAsync(IConnectionStringFactory connectionStringFactory)
        {
            return _connectionFactory.OpenAsync(connectionStringFactory);
        }

        public DbTransaction BeginTransaction(DbConnection dbConnection)
        {
            return _transactionFactory.BeginTransaction(dbConnection);
        }

        public DbTransaction BeginTransaction(DbConnection dbConnection, IsolationLevel isolationLevel)
        {
            return _transactionFactory.BeginTransaction(dbConnection, isolationLevel);
        }

        public DbParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction)
        {
            return _parameterFactory.CreateParameter(name, value, type, direction);
        }

        public DbParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction, int size)
        {
            return _parameterFactory.CreateParameter(name, value, type, direction, size);
        }

   }
}
