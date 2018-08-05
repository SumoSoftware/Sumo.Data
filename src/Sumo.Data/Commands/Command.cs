using Sumo.Data.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Commands
{
    //todo: add tests
    public class Command : ICommand
    {
        public Command(DbConnection dbConnection, IParameterFactory parameterFactory)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
            _dbCommand = _dbConnection.CreateCommand();
            _dbCommand.CommandType = CommandType.Text;
        }

        protected readonly DbConnection _dbConnection;
        protected readonly IParameterFactory _parameterFactory;
        protected readonly DbCommand _dbCommand;
        protected bool IsPrepared { get; private set; } = false;

        protected void InternalPrepare(Dictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var index = -1;
            foreach (var item in parameters)
            {
                var name = _parameterFactory.GetParameterName(item.Key, index++);
                var value = item.Value ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, ParameterDirection.Input);
                _dbCommand.Parameters.Add(parameter);
            }
        }

        protected void InternalSetParameterValues(Dictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var index = -1;
            foreach (var item in parameters)
            {
                var name = _parameterFactory.GetParameterName(item.Key, index++);
                var parameter = _dbCommand.Parameters[name];
                if (parameter == null) throw new InvalidOperationException($"Command parameter with name '{name}' not found.");
                parameter.Value = item.Value ?? DBNull.Value;
            }
        }

        /// <summary>
        /// returns did prepare
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool Prepare(string sql, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            var didPrepare = !IsPrepared;
            if (!IsPrepared)
            {
                _dbCommand.CommandText = sql;
                if (parameters != null) InternalPrepare(parameters);
                //todo: can't prepare for MS Sql Server unless all the params have an explicit type set from DbType enumeration
                //Command.Prepare();
                IsPrepared = true;
            }

            return didPrepare;
        }

        public void SetParameterValues(string sql, Dictionary<string, object> parameters = null)
        {
            if (!Prepare(sql, parameters) && (parameters != null))
            {
                InternalSetParameterValues(parameters);
            }
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

        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            SetParameterValues(sql, parameters);

            return await _dbCommand.ExecuteNonQueryAsync();
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

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _dbCommand.Dispose();
                }
                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
