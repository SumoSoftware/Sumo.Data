using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public class CommandProcedure : Procedure, ICommandProcedure
    {
        public CommandProcedure(DbConnection dbConnection, IParameterFactory parameterFactory) : base(dbConnection, parameterFactory) { }

        public CommandProcedure(IDataComponentFactory factory) : base(factory) { }

        public long Execute<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            SetParameterValues(procedureParams);
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
            _dbCommand.ExecuteNonQuery();
            FillOutputParameters(procedureParams);
            return GetProcedureResult();
        }

        public async Task<long> ExecuteAsync<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            return await Task.Run(async () =>
            {
                SetParameterValues(procedureParams);
                if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;
                await _dbCommand.ExecuteNonQueryAsync();
                FillOutputParameters(procedureParams);
                return GetProcedureResult();
            });
        }
    }
}
