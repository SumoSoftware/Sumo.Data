using System;
using System.Data;
using System.Data.Common;

namespace Sumo.Data.Factories.Sqlite
{
    public class SqliteTransactionFactory : ITransactionFactory
    {
        public DbTransaction BeginTransaction(DbConnection dbConnection)
        {
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            return dbConnection.BeginTransaction();            
        }
        
        public DbTransaction BeginTransaction(DbConnection dbConnection, IsolationLevel isolationLevel)
        {
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            return dbConnection.BeginTransaction(isolationLevel);
        }
    }
}
