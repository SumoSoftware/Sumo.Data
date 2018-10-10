using Sumo.Retry;
using System;
using System.Data;
using System.Data.Common;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerTransactionFactoryWithRetry : ITransactionFactory
    {
        private readonly ITransactionFactory _proxy;

        public SqlServerTransactionFactoryWithRetry(SqlServerTransientRetryPolicy retryPolicy)
        {
            if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));
            
            _proxy = RetryProxy.Create<ITransactionFactory>(
                new TransactionFactory(),
                retryPolicy);
        }

        public SqlServerTransactionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new SqlServerTransientRetryPolicy(maxAttempts, timeout))
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
