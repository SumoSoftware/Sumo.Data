using Sumo.Data.Exceptions.Sqlite;
using Sumo.Data.Factories;
using Sumo.Data.Factories.Sqlite;
using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Readers.Sqlite
{
    public sealed class SqliteSqlReaderWithRetry : ISqlReader
    {
        private readonly ISqlReader _proxy;

        public SqliteSqlReaderWithRetry(DbConnection dbConnection, RetryOptions retryOptions)
        {
            var instance = new SqlReader(dbConnection, new SqliteParameterFactory(), new SqliteDataAdapterFactory());
            _proxy = RetryProxy.Create<ISqlReader>(instance, retryOptions, new SqliteTransientErrorTester());
        }

        public SqliteSqlReaderWithRetry(DbConnection dbConnection, IDataAdapterFactory dataAdapterFactory, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new RetryOptions(maxAttempts, timeout))
        { }

        public void Dispose()
        {
            _proxy.Dispose();
        }

        public bool Prepare(string sql, Dictionary<string, object> queryParams = null)
        {
            return _proxy.Prepare(sql, queryParams);
        }

        public DataSet Read(string sql, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(sql, dbTransaction);
        }

        public DataSet Read(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(sql, parameters, dbTransaction);
        }

        public Task<DataSet> ReadAsync(string sql, DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(sql, dbTransaction);
        }

        public Task<DataSet> ReadAsync(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(sql, parameters, dbTransaction);
        }

        public void SetParameterValues(string sql, Dictionary<string, object> queryParams = null)
        {
            _proxy.SetParameterValues(sql, queryParams);
        }
    }
}
