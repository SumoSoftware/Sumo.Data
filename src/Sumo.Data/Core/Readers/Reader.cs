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
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
            _dataAdapterFactory = dataAdapterFactory ?? throw new ArgumentNullException(nameof(dataAdapterFactory));
            _command = _dbConnection.CreateCommand();
            _command.CommandType = CommandType.Text;
        }

        protected readonly DbConnection _dbConnection;
        protected readonly IParameterFactory _parameterFactory;
        protected readonly DbCommand _command;
        protected readonly IDataAdapterFactory _dataAdapterFactory;
        protected bool IsPrepared { get; private set; } = false;

        protected void InternalPrepare(Dictionary<string, object> queryParams)
        {
            var index = -1;
            foreach (var item in queryParams)
            {
                var name = _parameterFactory.GetParameterName(item.Key, index++).ToString();
                var value = item.Value ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, ParameterDirection.Input);
                _command.Parameters.Add(parameter);
            }
        }

        protected void InternalSetParameterValues(Dictionary<string, object> queryParams)
        {
            var index = -1;
            foreach (var item in queryParams)
            {
                var name = _parameterFactory.GetParameterName(item.Key, index++).ToString();
                var parameter = _command.Parameters[name];
                if (parameter == null) throw new InvalidOperationException($"Command parameter with name '{name}' not found.");
                parameter.Value = item.Value ?? DBNull.Value;
            }
        }

        /// <summary>
        /// returns did prepare
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public bool Prepare(string sql, Dictionary<string, object> queryParams = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            var result = !IsPrepared;
            if (!IsPrepared)
            {
                _command.CommandText = sql;
                InternalPrepare(queryParams);
                //todo: can't prepare unless all the params have an explicit type set from DbType enumeration
                //Command.Prepare();
                IsPrepared = true;
            }

            return result;
        }

        public void SetParameterValues(string sql, Dictionary<string, object> queryParams = null)
        {
            if (!Prepare(sql, queryParams))
            {
                InternalSetParameterValues(queryParams);
            }
        }

        protected DataSet ExecuteCommand(DbTransaction dbTransaction)
        {
            if (_command.Transaction != dbTransaction) _command.Transaction = dbTransaction;

            var result = new DataSet();
            using (var dataAdapter = _dataAdapterFactory.CreateDataAdapter(_command))
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
                    _command.Dispose();
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
