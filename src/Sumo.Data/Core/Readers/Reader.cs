using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Sumo.Data
{
    public class Reader : IReader
    {
        public Reader(DbConnection dbConnection, IParameterFactory parameterFactory, IDataAdapterFactory dataAdapterFactory)
        {
            _dataAdapterFactory = dataAdapterFactory ?? throw new ArgumentNullException(nameof(dataAdapterFactory));
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));
            _dbCommand = dbConnection.CreateCommand();
            _dbCommand.CommandType = CommandType.Text;
        }

        protected readonly DbCommand _dbCommand;
        protected readonly IParameterFactory _parameterFactory;
        protected readonly IDataAdapterFactory _dataAdapterFactory;
        protected bool IsPrepared { get; private set; } = false;

        protected void InternalPrepare(Dictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var index = -2;
            foreach (var item in parameters)
            {
                var name = _parameterFactory.GetParameterName(item.Key, ++index).ToString();
                var value = item.Value ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, ParameterDirection.Input);
                _dbCommand.Parameters.Add(parameter);
            }
        }

        protected void InternalSetParameterValues(Dictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var index = -2;
            foreach (var item in parameters)
            {
                var name = _parameterFactory.GetParameterName(item.Key, ++index).ToString();
                var parameter = _dbCommand.Parameters[name];
                if (parameter == null) throw new InvalidOperationException($"Parameter with name '{name}' not found.");
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
                //todo: can't prepare unless all the params have an explicit type set from DbType enumeration
                _dbCommand.Prepare();
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

        protected DataSet ExecuteCommand(DbTransaction dbTransaction)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;

            var result = new DataSet();
            using (var dataAdapter = _dataAdapterFactory.CreateDataAdapter(_dbCommand))
            {
                dataAdapter.Fill(result);
            }
            return result;
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
