using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public abstract class DataProviderFactory : IDataProviderFactory
    {
        public abstract DbTransaction BeginTransaction(DbConnection dbConnection);
        public abstract DbTransaction BeginTransaction(DbConnection dbConnection, IsolationLevel isolationLevel);
        public abstract DbDataAdapter CreateDataAdapter(DbCommand dbCommand);
        public abstract DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size);
        public abstract DbParameter CreateParameter(string name, object value, ParameterDirection direction);
        public abstract DbParameter CreateReturnParameter(string name);
        public abstract string GetParameterName(string name, int index);
        public abstract DbConnection Open(string connectionString);
        public abstract DbConnection Open();
        public abstract Task<DbConnection> OpenAsync(string connectionString);
        public abstract Task<DbConnection> OpenAsync();
    }
}
