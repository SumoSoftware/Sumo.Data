using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public class SqlReader : Reader, ISqlReader
    {
        public SqlReader(DbConnection dbConnection, IParameterFactory parameterFactory, IDataAdapterFactory dataAdapterFactory) :
            base(dbConnection, parameterFactory, dataAdapterFactory)
        { }

        public DataSet Read(string sql, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            _command.CommandText = sql;
            return ExecuteCommand(dbTransaction);
        }

        public async Task<DataSet> ReadAsync(string sql, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            return await Task.Run(() => { return Read(sql, dbTransaction); });
        }

        public DataSet Read(string sql, Dictionary<string, object> queryParams, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            SetParameterValues(sql, queryParams);
            return ExecuteCommand(dbTransaction);
        }

        public async Task<DataSet> ReadAsync(string sql, Dictionary<string, object> queryParams, DbTransaction dbTransaction = null)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            return await Task.Run(() => { return Read(sql, queryParams, dbTransaction); });
        }
    }
}
