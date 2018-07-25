using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Queries
{
    public interface IQueryReader: IReader
    {
        DataSet Read(QueryExpression query, DbTransaction dbTransaction = null);
        Task<DataSet> ReadAsync(QueryExpression query, DbTransaction dbTransaction = null);
    }
}