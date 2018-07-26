using Sumo.Data.Exceptions.Sqlite;
using Sumo.Data.Factories.Sqlite;
using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Queries.Sqlite
{
    public sealed class SqliteQueryReaderWithRetry : IQueryReader
    {
        private readonly IQueryReader _proxy;

        public SqliteQueryReaderWithRetry(DbConnection dbConnection, RetryOptions retryOptions)
        {
            var instance = new QueryReader(dbConnection, new SqliteParameterFactory(), new SqliteDataAdapterFactory());
            _proxy = Retry.Create<IQueryReader>(instance, retryOptions, new SqliteTransientErrorTester());
        }

        public SqliteQueryReaderWithRetry(DbConnection dbConnection, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new RetryOptions(maxAttempts, timeout))
        { }

        public DataSet Read(QueryExpression query, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(query, dbTransaction);
        }

        public Task<DataSet> ReadAsync(QueryExpression query, DbTransaction dbTransaction = null)
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
