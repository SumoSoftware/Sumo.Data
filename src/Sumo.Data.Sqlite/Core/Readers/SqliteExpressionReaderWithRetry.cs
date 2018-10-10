using Sumo.Data.Expressions;
using Sumo.Data.SqlExpressions;
using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Data.Sqlite
{
    public sealed class SqliteExpressionReaderWithRetry : IExpressionReader
    {
        private readonly IExpressionReader _proxy;

        public SqliteExpressionReaderWithRetry(DbConnection dbConnection, SqliteTransientRetryPolicy retryPolicy)
        {
            var instance = new ExpressionReader(dbConnection, new SqliteParameterFactory(), new SqliteDataAdapterFactory());
            _proxy = RetryProxy.Create<IExpressionReader>(instance, retryPolicy);
        }

        public SqliteExpressionReaderWithRetry(DbConnection dbConnection, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new SqliteTransientRetryPolicy(maxAttempts, timeout))
        { }

        public DataSet Read(SqlExpression query, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(query, dbTransaction);
        }

        public Task<DataSet> ReadAsync(SqlExpression query, DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(query, dbTransaction);
        }

        public bool Prepare(string sql, Dictionary<string, object> parameters = null)
        {
            return _proxy.Prepare(sql, parameters);
        }

        public void SetParameterValues(string sql, Dictionary<string, object> parameters = null)
        {
            _proxy.SetParameterValues(sql, parameters);
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }

        public DataSet Read(IExpression expression, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(expression, dbTransaction);
        }

        public Task<DataSet> ReadAsync(IExpression expression, DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(expression, dbTransaction);
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
    }
}
