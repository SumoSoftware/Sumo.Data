using Sumo.Data.SqlExpressions;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public interface IExpressionReader: IReader
    {
        DataSet Read(SqlExpression expression, DbTransaction dbTransaction = null);
        Task<DataSet> ReadAsync(SqlExpression expression, DbTransaction dbTransaction = null);

        DataSet Read(IExpression expression, DbTransaction dbTransaction = null);
        Task<DataSet> ReadAsync(IExpression expression, DbTransaction dbTransaction = null);
    }
}