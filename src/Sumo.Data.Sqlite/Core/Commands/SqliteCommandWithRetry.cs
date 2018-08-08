using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Sqlite
{
    class SqliteCommandWithRetry: ICommand
    {
        private readonly ICommand _proxy;

        public SqliteCommandWithRetry(DbConnection dbConnection, RetryOptions retryOptions)
        {
            var instance = new Command(dbConnection, new SqliteParameterFactory());
            _proxy = RetryProxy.Create<ICommand>(instance, retryOptions, new SqliteTransientErrorTester());
        }

        public SqliteCommandWithRetry(DbConnection dbConnection, int maxAttempts, TimeSpan timeout) :
            this(dbConnection, new RetryOptions(maxAttempts, timeout))
        { }

        public void Dispose()
        {
            _proxy.Dispose();
        }

        public int Execute(string sql, DbTransaction dbTransaction = null)
        {
            return _proxy.Execute(sql, dbTransaction);
        }

        public int Execute(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.Execute(sql, parameters, dbTransaction);
        }

        public int Execute(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.Execute(parameters, dbTransaction);
        }

        public Task<int> ExecuteAsync(string sql, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteAsync(sql, dbTransaction);
        }

        public Task<int> ExecuteAsync(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteAsync(sql, parameters, dbTransaction);
        }

        public Task<int> ExecuteAsync(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteAsync(parameters, dbTransaction);
        }

        public T ExecuteScalar<T>(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalar<T>(sql, parameters, dbTransaction);
        }

        public T ExecuteScalar<T>(string sql, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalar<T>(sql, dbTransaction);
        }

        public T ExecuteScalar<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalar<T>(parameters, dbTransaction);
        }

        public T ExecuteScalar<T>(DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalar<T>(dbTransaction);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalarAsync<T>(sql, parameters, dbTransaction);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalarAsync<T>(sql, dbTransaction);
        }

        public Task<T> ExecuteScalarAsync<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalarAsync<T>(parameters, dbTransaction);
        }

        public Task<T> ExecuteScalarAsync<T>(DbTransaction dbTransaction = null)
        {
            return _proxy.ExecuteScalarAsync<T>(dbTransaction);
        }

        public bool Prepare(string sql, Dictionary<string, object> queryParams = null)
        {
            return _proxy.Prepare(sql, queryParams);
        }

        public void SetParameterValues(string sql, Dictionary<string, object> queryParams = null)
        {
            _proxy.SetParameterValues(sql, queryParams);
        }
    }
}
