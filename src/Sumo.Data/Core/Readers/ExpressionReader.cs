using Sumo.Data.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public class ExpressionReader : Reader, IExpressionReader
    {
        public ExpressionReader(DbConnection dbConnection, IParameterFactory parameterFactory, IDataAdapterFactory dataAdapterFactory) :
            base(dbConnection, parameterFactory, dataAdapterFactory)
        { }

        private SqlStatement _sqlStatement;
        private SqlExpression _query;

        private class SqlStatement
        {
            public SqlStatement(string sql, Dictionary<string, object> parameters)
            {
                Sql = sql ?? throw new ArgumentNullException(nameof(sql));
                Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            }

            public string Sql { get; }
            public Dictionary<string, object> Parameters { get; }

            public static implicit operator SqlStatement(SqlExpression query)
            {
                return new SqlStatement(query.ToString(), query.Expression.GetParameters());
            }
        }

        private SqlStatement GetSqlStatement(SqlExpression query)
        {
            //tood: add iequatable to the query, table and expression classes
            if (!query.Equals(_query))
            {
                _query = query;
                _sqlStatement = query;
            }
            return _sqlStatement;
        }

        public DataSet Read(SqlExpression expression, DbTransaction dbTransaction = null)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var sqlStatement = GetSqlStatement(expression);
            SetParameterValues(sqlStatement.Sql, sqlStatement.Parameters);

            return ExecuteCommand(dbTransaction);
        }

        public async Task<DataSet> ReadAsync(SqlExpression expression, DbTransaction dbTransaction = null)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return await Task.Run(() => { return Read(expression, dbTransaction); });
        }

        public DataSet Read(IExpression expression, DbTransaction dbTransaction = null)
        {
            //todo: implement method
            throw new NotImplementedException();
        }

        public Task<DataSet> ReadAsync(IExpression expression, DbTransaction dbTransaction = null)
        {
            //todo: implement method
            throw new NotImplementedException();
        }
    }
}
