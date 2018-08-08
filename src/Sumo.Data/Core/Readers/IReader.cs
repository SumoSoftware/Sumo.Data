using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public interface IReader : IPreparable, IDisposable
    {
        DbDataReader ExecuteReader(DbTransaction dbTransaction);
        Task<DbDataReader> ExecuteReaderAsync(DbTransaction dbTransaction);
        Task<DbDataReader> ExecuteReaderAsync(DbTransaction dbTransaction, CancellationToken cancellationToken);
    }
}
