using System.Data;
using System.Data.Common;

namespace Sumo.Data.Factories
{
    public interface ITransactionFactory
    {
        DbTransaction BeginTransaction(DbConnection dbConnection);
        DbTransaction BeginTransaction(DbConnection dbConnection, IsolationLevel isolationLevel);
    }
}
