using Sumo.Data.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Queries
{
    public class QueryReader : Reader, IQueryReader
    {
        public QueryReader(DbConnection dbConnection, IParameterFactory parameterFactory, IDataAdapterFactory dataAdapterFactory) :
            base(dbConnection, parameterFactory, dataAdapterFactory)
        { }

        private SqlStatement _sqlStatement;
        private QueryExpression _query;

        private class SqlStatement
        {
            public SqlStatement(string sql, Dictionary<string, object> parameters)
            {
                Sql = sql ?? throw new ArgumentNullException(nameof(sql));
                Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            }

            public string Sql { get; }
            public Dictionary<string, object> Parameters { get; }

            public static implicit operator SqlStatement(QueryExpression query)
            {
                return new SqlStatement(query.ToString(), query.Expression.GetParameters());
            }
        }

        private SqlStatement GetSqlStatement(QueryExpression query)
        {
            //tood: add iequatable to the query, table and expression classes
            if (!query.Equals(_query))
            {
                _query = query;
                _sqlStatement = query;
            }
            return _sqlStatement;
        }

        public DataSet Read(QueryExpression query, DbTransaction dbTransaction = null)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var sqlStatement = GetSqlStatement(query);
            SetParameterValues(sqlStatement.Sql, sqlStatement.Parameters);

            return ExecuteCommand(dbTransaction);
        }

        public async Task<DataSet> ReadAsync(QueryExpression query, DbTransaction dbTransaction = null)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return await Task.Run(() => { return Read(query, dbTransaction); });
        }
    }
}
