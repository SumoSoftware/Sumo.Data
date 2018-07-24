using Sumo.Data.Exceptions.SqlServer;
using Sumo.Data.Factories.SqlServer;
using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Queries.SqlServer
{
    public sealed class SqlServerQueryReaderWithRetry : IQueryReader
    {
        private readonly IQueryReader _proxy;

        public SqlServerQueryReaderWithRetry(DbConnection dbConnection, RetryOptions retryOptions)
        {
            var instance = new QueryReader(dbConnection, new SqlServerParameterFactory(), new SqlServerDataAdapterFactory());
            _proxy = RetryProxy.Create<IQueryReader>(instance, retryOptions, new SqlServerTransientErrorTester());
        }

        public SqlServerQueryReaderWithRetry(DbConnection dbConnection, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new RetryOptions(maxAttempts, timeout))
        { }

        public DataSet Read(Query query, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(query, dbTransaction);
        }

        public Task<DataSet> ReadAsync(Query query, DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(query, dbTransaction);
        }

        public bool Prepare(string sql, Dictionary<string, object> queryParams = null)
        {
            return _proxy.Prepare(sql, queryParams);
        }

        public void SetParameterValues(string sql, Dictionary<string, object> queryParams = null)
        {
            _proxy.SetParameterValues(sql, queryParams);
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }
    }
}
