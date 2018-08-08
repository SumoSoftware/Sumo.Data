using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerSqlReaderWithRetry : ISqlReader
    {
        private readonly ISqlReader _proxy;

        public SqlServerSqlReaderWithRetry(DbConnection dbConnection, RetryOptions retryOptions)
        {
            var instance = new SqlReader(dbConnection, new SqlServerParameterFactory(), new SqlServerDataAdapterFactory());
            _proxy = RetryProxy.Create<ISqlReader>(instance, retryOptions, new SqlServerTransientErrorTester());
        }

        public SqlServerSqlReaderWithRetry(DbConnection dbConnection, IDataAdapterFactory dataAdapterFactory, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new RetryOptions(maxAttempts, timeout))
        { }

        public bool Prepare(string sql, Dictionary<string, object> parameters = null)
        {
            return _proxy.Prepare(sql, parameters);
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

        public void SetParameterValues(string sql, Dictionary<string, object> parameters = null)
        {
            _proxy.SetParameterValues(sql, parameters);
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }

        public DbDataReader ExecuteReader(DbTransaction dbTransaction)
        {
            return _proxy.ExecuteReader(dbTransaction);
        }

        public Task<DbDataReader> ExecuteReaderAsync(DbTransaction dbTransaction)
        {
            return _proxy.ExecuteReaderAsync(dbTransaction);
        }

        public Task<DbDataReader> ExecuteReaderAsync(DbTransaction dbTransaction, CancellationToken cancellationToken)
        {
            return _proxy.ExecuteReaderAsync(dbTransaction, cancellationToken);
        }

        public DataSet Read(DbTransaction dbTransaction = null)
        {
            return _proxy.Read(dbTransaction);
        }

        public Task<DataSet> ReadAsync(DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(dbTransaction);
        }

        public DataSet Read(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(parameters, dbTransaction);
        }

        public Task<DataSet> ReadAsync(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(parameters, dbTransaction);
        }
    }
}
