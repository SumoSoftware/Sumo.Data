using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public interface IReadProcedure : IProcedure
    {
        IProcedureReadResult Read<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class;

        Task<IProcedureReadResult> ReadAsync<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class;
    }
}
