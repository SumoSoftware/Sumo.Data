using Sumo.Retry;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerCommandProcedureWithRetry : ICommandProcedure
    {
        private readonly ICommandProcedure _proxy;

        public SqlServerCommandProcedureWithRetry(DbConnection dbConnection, RetryOptions retryOptions)
        {
            var instance = new CommandProcedure(dbConnection, new SqlServerParameterFactory());
            _proxy = RetryProxy.Create<ICommandProcedure>(instance, retryOptions, new SqlServerTransientErrorTester());
        }

        public SqlServerCommandProcedureWithRetry(DbConnection dbConnection, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new RetryOptions(maxAttempts, timeout))
        { }

        public SqlServerCommandProcedureWithRetry(IDataProviderFactory factory, RetryOptions retryOptions)
        {
            var instance = new CommandProcedure(factory);
            _proxy = RetryProxy.Create<ICommandProcedure>(instance, retryOptions, new SqlServerTransientErrorTester());
        }

        public long Execute<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            return _proxy.Execute(procedureParams, dbTransaction);
        }

        public async Task<long> ExecuteAsync<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            return await _proxy.ExecuteAsync(procedureParams, dbTransaction);
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
