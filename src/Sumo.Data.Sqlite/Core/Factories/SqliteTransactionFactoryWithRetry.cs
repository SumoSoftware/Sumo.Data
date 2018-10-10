using Sumo.Retry;
using System;
using System.Data;
using System.Data.Common;

namespace Sumo.Data.Sqlite
{
    public class SqliteTransactionFactoryWithRetry : ITransactionFactory
    {
        private readonly ITransactionFactory _proxy;
        
        public SqliteTransactionFactoryWithRetry(SqliteTransientRetryPolicy retryPolicy)
        {
            if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));

            _proxy = RetryProxy.Create<ITransactionFactory>(
                new TransactionFactory(),
                retryPolicy);
        }

        public SqliteTransactionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new SqliteTransientRetryPolicy(maxAttempts, timeout))
        { }

        public DbTransaction BeginTransaction(DbConnection dbConnection)
        {
            return _proxy.BeginTransaction(dbConnection);
        }

        public DbTransaction BeginTransaction(DbConnection dbConnection, IsolationLevel isolationLevel)
        {
            return _proxy.BeginTransaction(dbConnection, isolationLevel);
        }
    }
}
