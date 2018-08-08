using Sumo.Data.SqlExpressions;
using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerExpressionReaderWithRetry : IExpressionReader
    {
        private readonly IExpressionReader _proxy;

        public SqlServerExpressionReaderWithRetry(DbConnection dbConnection, RetryOptions retryOptions)
        {
            var instance = new ExpressionReader(dbConnection, new SqlServerParameterFactory(), new SqlServerDataAdapterFactory());
            _proxy = RetryProxy.Create<IExpressionReader>(instance, retryOptions, new SqlServerTransientErrorTester());
        }

        public SqlServerExpressionReaderWithRetry(DbConnection dbConnection, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new RetryOptions(maxAttempts, timeout))
        { }

        public DataSet Read(SqlExpression query, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(query, dbTransaction);
        }

        public Task<DataSet> ReadAsync(SqlExpression query, DbTransaction dbTransaction = null)
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

        public DataSet Read(IExpression expression, DbTransaction dbTransaction = null)
        {
            return _proxy.Read(expression, dbTransaction);
        }

        public Task<DataSet> ReadAsync(IExpression expression, DbTransaction dbTransaction = null)
        {
            return _proxy.ReadAsync(expression, dbTransaction);
        }
    }
}
