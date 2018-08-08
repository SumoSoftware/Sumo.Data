using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public interface ICommandProcedure : IProcedure
    {
        long Execute<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class;
        Task<long> ExecuteAsync<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class;
    }
}
