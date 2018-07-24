using Sumo.Data.Factories;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Procedures
{
    public class CommandProcedure : Procedure, ICommandProcedure
    {
        public CommandProcedure(DbConnection dbConnection, IParameterFactory parameterFactory) : 
            base(dbConnection, parameterFactory) { }

        public long Execute<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            SetParameterValues(procedureParams);
            if (_command.Transaction != dbTransaction) _command.Transaction = dbTransaction;
            _command.ExecuteNonQuery();
            FillOutputParameters(procedureParams);
            return GetProcedureResult();
        }

        public async Task<long> ExecuteAsync<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            return await Task.Run<long>(async () =>
            {
                SetParameterValues(procedureParams);
                if (_command.Transaction != dbTransaction) _command.Transaction = dbTransaction;
                await _command.ExecuteNonQueryAsync();
                FillOutputParameters(procedureParams);
                return GetProcedureResult();
            });
        }
    }
}
