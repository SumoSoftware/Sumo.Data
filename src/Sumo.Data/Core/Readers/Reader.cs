using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public class Reader : Preparable, IReader
    {
        public Reader(DbConnection dbConnection, IParameterFactory parameterFactory) : base(dbConnection, parameterFactory)
        {
            throw new NotSupportedException($"{nameof(Reader)} doesn't support the constructor with this signature: {nameof(Reader)}({nameof(DbConnection)} {nameof(dbConnection)}, {nameof(IParameterFactory)} {nameof(parameterFactory)})");
        }

        public Reader(IDataComponentFactory factory) : base(factory)
        {
            throw new NotSupportedException($"{nameof(Reader)} doesn't support the constructor with this signature: {nameof(Reader)}({nameof(IDataComponentFactory)} {nameof(factory)})");
        }

        public Reader(DbConnection dbConnection, IParameterFactory parameterFactory, IDataAdapterFactory dataAdapterFactory) : base(dbConnection, parameterFactory)
        {
            _dataAdapterFactory = dataAdapterFactory ?? throw new ArgumentNullException(nameof(dataAdapterFactory));
        }

        public Reader(DbConnection dbConnection, IDataComponentFactory factory) : base(factory)
        {
            _dataAdapterFactory = factory;
        }

        protected readonly IDataAdapterFactory _dataAdapterFactory;

        protected DataSet ExecuteCommand(DbTransaction dbTransaction)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;

            var result = new DataSet();
            using (var dataAdapter = _dataAdapterFactory.CreateDataAdapter(_dbCommand))
            {
                //dataAdapter.FillSchema(null, SchemaType.Mapped)
                dataAdapter.Fill(result);
            }
            return result;
        }

        public DbDataReader ExecuteReader(DbTransaction dbTransaction)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;

            return _dbCommand.ExecuteReader();
        }

        public Task<DbDataReader> ExecuteReaderAsync(DbTransaction dbTransaction)
        {
            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;

            return _dbCommand.ExecuteReaderAsync();
        }

        public Task<DbDataReader> ExecuteReaderAsync(DbTransaction dbTransaction, CancellationToken cancellationToken)
        {
            if (cancellationToken == null) throw new ArgumentNullException(nameof(cancellationToken));

            if (_dbCommand.Transaction != dbTransaction) _dbCommand.Transaction = dbTransaction;

            return _dbCommand.ExecuteReaderAsync(cancellationToken);
        }
    }
}
