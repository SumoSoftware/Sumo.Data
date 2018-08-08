using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public class ReadProcedure : Procedure, IReadProcedure
    {
        public ReadProcedure(DbConnection dbConnection, IParameterFactory parameterFactory, IDataAdapterFactory dataAdapterFactory) :
            base(dbConnection, parameterFactory)
        {
            _dataAdapter = dataAdapterFactory.CreateDataAdapter(_command);
        }

        public ReadProcedure(IDataComponentFactory factory) : base(factory)
        {
            _dataAdapter = factory.CreateDataAdapter(_command);
        }

        public ReadProcedure(DbConnection dbConnection, IDataComponentFactory factory) :
            base(dbConnection, factory)
        {
            _dataAdapter = factory.CreateDataAdapter(_command);
        }

        internal readonly DbDataAdapter _dataAdapter;

        private DataSet ExecuteCommand<P>(P procedureParams, DbTransaction dbTransaction) where P : class
        {
            if (_command.Transaction != dbTransaction) _command.Transaction = dbTransaction;
            var result = new DataSet();
            _dataAdapter.Fill(result);
            FillOutputParameters(procedureParams);
            return result;
        }

        public IProcedureReadResult Read<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            SetParameterValues(procedureParams);
            var dataSet = ExecuteCommand(procedureParams, dbTransaction);
            return new ProcedureReadResult(dataSet, GetProcedureResult());
        }

        public async Task<IProcedureReadResult> ReadAsync<P>(P procedureParams, DbTransaction dbTransaction = null) where P : class
        {
            return await Task.Run(() => { return Read(procedureParams, dbTransaction); });
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _dataAdapter.Dispose();
                    base.Dispose(disposing);
                }
                _disposedValue = true;
            }
        }
        #endregion
    }
}
