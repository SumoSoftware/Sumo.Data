using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Sumo.Data
{
    public abstract class Preparable : IPreparable, IDisposable
    {
        public Preparable(DbConnection dbConnection, IParameterFactory parameterFactory)
        {
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));

            _dbCommand = dbConnection.CreateCommand();
            _dbCommand.CommandType = CommandType.Text;
        }

        public Preparable(IDataComponentFactory factory) : this(factory.Open(), factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _ownsConnection = true;
        }

        public bool IsPrepared { get; private set; } = false;

        protected readonly DbCommand _dbCommand;
        protected readonly IParameterFactory _parameterFactory;

        private readonly bool _ownsConnection = false;

        protected void InternalPrepare(Dictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var index = -2;
            foreach (var item in parameters)
            {
                var name = _parameterFactory.GetParameterName(item.Key, ++index);
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
                var name = _parameterFactory.GetParameterName(item.Key, ++index);
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
            if (String.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            var didPrepare = !IsPrepared;
            if (!IsPrepared)
            {
                _dbCommand.CommandText = sql;
                if (parameters != null && parameters.Count > 0)
                {
                    InternalPrepare(parameters);
                    _dbCommand.Prepare();
                    IsPrepared = true;
                }
                
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

        //public bool Prepare<P>(P procedureParams) where P : class
        //{
        //    //todo: is there a way to support this?
        //    throw new NotImplementedException();
        //}

        //public void SetParameterValues<P>(P procedureParams) where P : class
        //{
        //    //todo: is there a way to support this?
        //    throw new NotImplementedException();
        //}

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    var connection = _dbCommand.Connection;
                    _dbCommand.Dispose();
                    if (_ownsConnection)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
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
