using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Readers
{
    public interface ISqlReader : IReader
    {
        DataSet Read(string sql, DbTransaction dbTransaction = null);
        Task<DataSet> ReadAsync(string sql, DbTransaction dbTransaction = null);

        DataSet Read(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null);
        Task<DataSet> ReadAsync(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null);
    }
}
