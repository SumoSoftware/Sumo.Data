﻿using Sumo.Data.Attributes;
using Sumo.Data.Factories;
using Sumo.Data.Types;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Sumo.Data.Procedures
{
    public abstract class Procedure : IProcedure
    {
        public Procedure(DbConnection dbConnection, IParameterFactory parameterFactory)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
            _command = _dbConnection.CreateCommand();
            _command.CommandType = CommandType.StoredProcedure;
        }

        internal Procedure(IConnectionFactory factory)
        {
            _parameterFactory = factory.ParameterFactory;

            //TODO: Should this be moved to encapsulate the actual execute emthod?  Right now dispose will do cleanup
            _dbConnection = factory.Open();
            _command = _dbConnection.CreateCommand();
            _command.CommandType = CommandType.StoredProcedure;
            
            
        }

        private readonly DbConnection _dbConnection;
        internal readonly DbCommand _command;
        internal readonly IParameterFactory _parameterFactory;

        private bool _isPrepared = false;

        public bool Prepare<P>(P procedureParams) where P : class
        {
            var result = !_isPrepared;
            if (!_isPrepared)
            {
                _command.CommandText = ProcedureParametersTypeInfoCache<P>.ProcedureName;
                InternalPrepare(procedureParams);
                _command.Prepare();
                _isPrepared = true;
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
                var value = GetOutputParameterValue(propertyInfo);
                if (value != null && !(value is DBNull)) propertyInfo.SetValue(procedureParams, value);
            }

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.OutputParameters.Length; ++i)
            {
                var propertyInfo = ProcedureParametersTypeInfoCache<P>.OutputParameters[i];
                var value = GetOutputParameterValue(propertyInfo);
                if (value != null && !(value is DBNull)) propertyInfo.SetValue(procedureParams, value);
            }
        }

        public static int GetParameterSize(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var outputAttribute = property.GetCustomAttribute<ParameterSizeAttribute>(true);
            if (outputAttribute == null) throw new InvalidOperationException($"Property '{property.Name}' does not contain a descendent ParameterSizeAttribute.");

            return outputAttribute.ParameterSize;
        }

        internal virtual string GetReturnValueParameterName()
        {
            return _parameterFactory.GetParameterName("Return_Value", -1);
        }

        internal virtual string GetParameterName(PropertyInfo property)
        {
            return _parameterFactory.GetParameterName(property.Name, -1);
        }

        internal object GetOutputParameterValue(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var paramName = GetParameterName(property);
            var parameter = _command.Parameters[paramName];

            if (parameter == null) throw new ArgumentException($"Parameter with name '{paramName}' not found.");

            return parameter.Value;
        }

        internal long GetProcedureResult()
        {
            var parameter = _command.Parameters[GetReturnValueParameterName()];
            return parameter != null ? Convert.ToInt64(parameter.Value) : 0L;
        }

        internal void InternalPrepare<P>(P procedureParams) where P : class
        {
            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputParameters[i];
                var name = GetParameterName(property);
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, ParameterDirection.Input);
                _command.Parameters.Add(parameter);
            }

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputOutputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputOutputParameters[i];
                var name = GetParameterName(property);
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, ParameterDirection.InputOutput, GetParameterSize(property));
                _command.Parameters.Add(parameter);
            }

            //todo: see SqlServerProcedureHelper.vb line 55 for how to do list based input parameters (table input to procedures)

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.OutputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.OutputParameters[i];
                var name = GetParameterName(property);
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                var parameter = _parameterFactory.CreateParameter(name, value, ParameterDirection.Output, GetParameterSize(property));
                _command.Parameters.Add(parameter);
            }

            var returnParameter = _parameterFactory.CreateReturnParameter(GetReturnValueParameterName());
            _command.Parameters.Add(returnParameter);
        }

        internal void InternalSetParameterValues<P>(P procedureParams) where P : class
        {
            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputParameters[i];
                var name = GetParameterName(property);
                var parameter = _command.Parameters[name];
                if (parameter == null) throw new InvalidOperationException($"Command parameter with name '{name}' not found.");
                var value = property.GetValue(procedureParams) ?? DBNull.Value;
                parameter.Value = value;
            }

            for (var i = 0; i < ProcedureParametersTypeInfoCache<P>.InputOutputParameters.Length; ++i)
            {
                var property = ProcedureParametersTypeInfoCache<P>.InputOutputParameters[i];
                var name = GetParameterName(property);
                var parameter = _command.Parameters[name];
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
                    _command.Dispose();
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
