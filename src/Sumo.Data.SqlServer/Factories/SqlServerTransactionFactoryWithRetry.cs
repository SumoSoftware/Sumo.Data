using Sumo.Retry;
using System;
using System.Data;
using System.Data.Common;

namespace Sumo.Data.SqlServer
{
    public sealed class SqlServerTransactionFactoryWithRetry : ITransactionFactory
    {
        private readonly ITransactionFactory _proxy;

        public SqlServerTransactionFactoryWithRetry(RetryOptions retryOptions)
        {
            if (retryOptions == null) throw new ArgumentNullException(nameof(retryOptions));
            
            _proxy = RetryProxy.Create<ITransactionFactory>(
                new TransactionFactory(),
                retryOptions,
                new SqlServerTransientErrorTester());
        }

        public SqlServerTransactionFactoryWithRetry(int maxAttempts, TimeSpan timeout) :
            this(new RetryOptions(maxAttempts, timeout))
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
