using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    //todo: add tests
    public class Command : Preparable, ICommand
    {
        public Command(string sql, DbConnection dbConnection, IParameterFactory parameterFactory) : base(dbConnection, parameterFactory)
        {
            _dbCommand.CommandText = sql;
        }

        public Command(DbConnection dbConnection, IParameterFactory parameterFactory) : base(dbConnection, parameterFactory)
        {
        }

        public Command(IDataComponentFactory factory) : base(factory)
        {
        }

        public int Execute(string sql, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            _dbCommand.CommandText = sql;
            return _dbCommand.ExecuteNonQuery();
        }

        public async Task<int> ExecuteAsync(string sql, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            _dbCommand.CommandText = sql;
            return await _dbCommand.ExecuteNonQueryAsync();
        }

        public int Execute(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            SetParameterValues(sql, parameters);

            return _dbCommand.ExecuteNonQuery();
        }

        public Task<int> ExecuteAsync(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            SetParameterValues(sql, parameters);

            return _dbCommand.ExecuteNonQueryAsync();
        }

        public T ExecuteScalar<T>(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            SetParameterValues(sql, parameters);

            var result = _dbCommand.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            SetParameterValues(sql, parameters);

            var result = await _dbCommand.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public T ExecuteScalar<T>(string sql, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            _dbCommand.CommandText = sql;

            var result = _dbCommand.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            _dbCommand.CommandText = sql;

            var result = await _dbCommand.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public int Execute(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            if (IsPrepared)
                SetParameterValues(parameters);
            else
                Prepare(_dbCommand.CommandText, parameters);
            return _dbCommand.ExecuteNonQuery();
        }

        public Task<int> ExecuteAsync(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            if (IsPrepared)
                SetParameterValues(parameters);
            else
                Prepare(_dbCommand.CommandText, parameters);
            return _dbCommand.ExecuteNonQueryAsync();
        }

        public T ExecuteScalar<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            if (IsPrepared)
                SetParameterValues(parameters);
            else
                Prepare(_dbCommand.CommandText, parameters);
            var result = _dbCommand.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task<T> ExecuteScalarAsync<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            if (IsPrepared)
                SetParameterValues(parameters);
            else
                Prepare(_dbCommand.CommandText, parameters);
            var result = await _dbCommand.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public T ExecuteScalar<T>(DbTransaction dbTransaction = null)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            Prepare(_dbCommand.CommandText);
            var result = _dbCommand.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task<T> ExecuteScalarAsync<T>(DbTransaction dbTransaction = null)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            Prepare(_dbCommand.CommandText);
            var result = await _dbCommand.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
