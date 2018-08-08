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
            if (String.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            Prepare(sql);
            return ExecuteCommand(dbTransaction);
        }

        public Task<DataSet> ReadAsync(string sql, DbTransaction dbTransaction = null)
        {
            if (String.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            return Task.Run(() => { return Read(sql, dbTransaction); });
        }

        public DataSet Read(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (String.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            SetParameterValues(sql, parameters);
            return ExecuteCommand(dbTransaction);
        }

        public async Task<DataSet> ReadAsync(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            if (String.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            return await Task.Run(() => { return Read(sql, parameters, dbTransaction); });
        }

        public DataSet Read(DbTransaction dbTransaction = null)
        {
            return ExecuteCommand(dbTransaction);
        }

        public Task<DataSet> ReadAsync(DbTransaction dbTransaction = null)
        {
            return Task.Run(() => { return Read(dbTransaction); });
        }

        public DataSet Read(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            InternalSetParameterValues(parameters);
            return ExecuteCommand(dbTransaction);
        }

        public Task<DataSet> ReadAsync(Dictionary<string, object> parameters, DbTransaction dbTransaction = null)
        {
            return Task.Run(() => { return Read(parameters, dbTransaction); });
        }
    }
}
