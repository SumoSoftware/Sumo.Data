using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema.SqlServer
{
    internal class SqlServerSqlStatementBuilder : ISqlStatementBuilder
    {
        public SqlServerSqlStatementBuilder(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
        }

        private readonly IParameterFactory _parameterFactory;


        public string GetExistsStatement<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public string GetInsertStatement<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public string GetSelectStatement<T>(Dictionary<string, object> parameters) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}