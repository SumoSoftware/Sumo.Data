using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Sumo.Data
{
    public abstract class Procedure : IProcedure
    {
        public Procedure(DbConnection dbConnection, IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));
            _dbCommand = dbConnection.CreateCommand();
            _dbCommand.CommandType = CommandType.StoredProcedure;
        }

        public Procedure(IDataComponentFactory factory) : this(factory.Open(), factory)
        {
            _ownsConnection = true;
        }

        private readonly bool _ownsConnection = false;

        internal readonly DbCommand _dbCommand;
        internal readonly IParameterFactory _parameterFactory;

        public bool IsPrepared { get; private set; } = false;

        public bool Prepare<P>(P procedureParams) where P : class
        {
            var result = !IsPrepared;
            if (!IsPrepared)
            {
                _dbCommand.CommandText = ProcedureParametersTypeInfoCache<P>.ProcedureName;
                InternalPrepare(procedureParams);
                _dbCommand.Prepare();
                IsPrepared = true;
            }
            return result;
        }

        public void SetParameterValues<P>(P procedureParams) where P : class
        {
            if (!Prepare(procedureParams))
            {
                InternalSetParameterValues(procedureParams);
            }
        }

        protected void FillOutputParameters<P>(P procedureParams) where P : class
        {
            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputOutputParameters.Length; ++i)
            {
                var propertyInfo = ProcedureParametersTypeInfoCache<P>.InputOutputParameters[i];
                var value = GetOutputParameterValue(ProcedureParametersTypeInfoCache<P>.InputOutputParameterNames[i]);
                if (value != null && !(value is DBNull)) propertyInfo.SetValue(procedureParams, value);
            }

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.OutputParameters.Length; ++i)
            {
                var propertyInfo = ProcedureParametersTypeInfoCache<P>.OutputParameters[i];
                var value = GetOutputParameterValue(ProcedureParametersTypeInfoCache<P>.OutputParameterNames[i]);
                if (value != null && !(value is DBNull)) propertyInfo.SetValue(procedureParams, value);
            }
        }

        public static int GetParameterSize(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var outputAttribute = property.GetCustomAttribute<InputParameterAttribute>(true);
            if (outputAttribute == null) throw new InvalidOperationException($"Property '{property.Name}' does not contain a descendent ParameterSizeAttribute.");

            return outputAttribute.ParameterSize;
        }

        internal virtual string GetReturnValueParameterName()
        {
            return _parameterFactory.GetParameterName("Return_Value", -1);
        }

        //todo: this isn't going to work - it ignores the parameter name attributes
        internal virtual string GetParameterName(string name)
        {
            return _parameterFactory.GetParameterName(name, -1);
        }

        internal object GetOutputParameterValue(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var paramName = GetParameterName(name);
            var parameter = _dbCommand.Parameters[paramName];

            if (parameter == null) throw new ArgumentException($"Parameter with name '{paramName}' not found.");

            return parameter.Value;
        }

        internal long GetProcedureResult()
        {
            var parameter = _dbCommand.Parameters[GetReturnValueParameterName()];
            return parameter != null ? Convert.ToInt64(parameter.Value) : 0L;
        }

        internal void InternalPrepare<P>(P procedureParams) where P : class
        {
            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputParameters[i];
                var name = GetParameterName(ProcedureParametersTypeInfoCache<P>.InputParameterNames[i]);
                var dbType = ProcedureParametersTypeInfoCache<P>.InputDbTypes[i];
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, dbType, ParameterDirection.Input);
                _dbCommand.Parameters.Add(parameter);
            }

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputOutputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputOutputParameters[i];
                var name = GetParameterName(ProcedureParametersTypeInfoCache<P>.InputOutputParameterNames[i]);
                var dbType = ProcedureParametersTypeInfoCache<P>.InputOutputDbTypes[i];
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, dbType, ParameterDirection.InputOutput, GetParameterSize(property));
                _dbCommand.Parameters.Add(parameter);
            }

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.OutputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.OutputParameters[i];
                var name = GetParameterName(ProcedureParametersTypeInfoCache<P>.OutputParameterNames[i]);
                var dbType = ProcedureParametersTypeInfoCache<P>.OutputDbTypes[i];
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, dbType, ParameterDirection.Output, GetParameterSize(property));
                _dbCommand.Parameters.Add(parameter);
            }

            //todo: see SqlServerProcedureHelper.vb line 55 for how to do list based input parameters (table input to procedures)

            var returnParameter = _parameterFactory.CreateReturnParameter(GetReturnValueParameterName());
            _dbCommand.Parameters.Add(returnParameter);
        }

        internal void InternalSetParameterValues<P>(P procedureParams) where P : class
        {
            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputParameters[i];
                var name = GetParameterName(ProcedureParametersTypeInfoCache<P>.InputParameterNames[i]);
                var parameter = _dbCommand.Parameters[name];
                if (parameter == null) throw new InvalidOperationException($"Command parameter with name '{name}' not found.");
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                parameter.Value = value;
            }

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputOutputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputOutputParameters[i];
                var name = GetParameterName(ProcedureParametersTypeInfoCache<P>.InputOutputParameterNames[i]);
                var parameter = _dbCommand.Parameters[name];
                if (parameter == null) throw new InvalidOperationException($"Command parameter with name '{name}' not found.");
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                parameter.Value = value;
            }

            //todo: see SqlServerProcedureHelper.vb line 92 for how to do list based input parameters (table input to procedures)
        }

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
        public virtual void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}
