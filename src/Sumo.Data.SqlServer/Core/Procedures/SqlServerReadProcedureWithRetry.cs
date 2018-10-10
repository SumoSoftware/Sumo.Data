using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerReadProcedureWithRetry : IReadProcedure
    {
        private readonly IReadProcedure _proxy;

        public SqlServerReadProcedureWithRetry(DbConnection dbConnection, SqlServerTransientRetryPolicy retryPolicy)
        {
            var instance = new ReadProcedure(dbConnection, new SqlServerParameterFactory(), new SqlServerDataAdapterFactory());
            _proxy = RetryProxy.Create<IReadProcedure>(instance, retryPolicy);
        }

        public SqlServerReadProcedureWithRetry(DbConnection dbConnection, IDataAdapterFactory dataAdapterFactory, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new SqlServerTransientRetryPolicy(maxAttempts, timeout))
        { }

        public SqlServerReadProcedureWithRetry(IDataComponentFactory factory, SqlServerTransientRetryPolicy retryPolicy)
        {
            var instance = new ReadProcedure(factory);
            _proxy = RetryProxy.Create<IReadProcedure>(instance, retryPolicy);
        }

        public IProcedureReadResult Read<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            return _proxy.Read(procedureParams, dbTransaction);
        }

        public async Task<IProcedureReadResult> ReadAsync<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            return await _proxy.ReadAsync(procedureParams, dbTransaction);
        }

        public bool Prepare<P>(P procedureParams) where P : class
        {
            return _proxy.Prepare(procedureParams);
        }

        public void SetParameterValues<P>(P procedureParams) where P : class
        {
            _proxy.SetParameterValues(procedureParams);
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }
    }
}
