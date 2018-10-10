using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Retry;
using System;
using System.Data;

namespace Sumo.Data.SqlServer
{
    [TestClass]
    public class TransactionFactory_Tests
    {
        [TestMethod]
        public void BeginTransaction()
        {
            ITransactionFactory transactionFactory = new TransactionFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();
            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using(var transaction = transactionFactory.BeginTransaction(connection))
            {
                Assert.IsNotNull(transaction);
                Assert.AreEqual(connection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.ReadCommitted, transaction.IsolationLevel);
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void BeginTransaction_WithIsolationLevel()
        {
            ITransactionFactory transactionFactory = new TransactionFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();
            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var transaction = transactionFactory.BeginTransaction(connection, IsolationLevel.Snapshot))
            {
                Assert.IsNotNull(transaction);
                Assert.AreEqual(connection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.Snapshot, transaction.IsolationLevel);
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void BeginTransaction_WithRetry()
        {
            var retryPolicy = new SqlServerTransientRetryPolicy(10, TimeSpan.FromSeconds(60));

            ITransactionFactory transactionFactory = new SqlServerTransactionFactoryWithRetry(retryPolicy);
            IConnectionFactory connectionFactory = new SqlServerConnectionFactoryWithRetry(retryPolicy);
            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var transaction = transactionFactory.BeginTransaction(connection))
            {
                Assert.IsNotNull(transaction);
                Assert.AreEqual(connection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.ReadCommitted, transaction.IsolationLevel);
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void BeginTransaction_WithIsolationLevel_WithRetry()
        {
            var retryPolicy = new SqlServerTransientRetryPolicy(10, TimeSpan.FromSeconds(60));

            ITransactionFactory transactionFactory = new SqlServerTransactionFactoryWithRetry(retryPolicy);
            IConnectionFactory connectionFactory = new SqlServerConnectionFactoryWithRetry(retryPolicy);
            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var transaction = transactionFactory.BeginTransaction(connection, IsolationLevel.Snapshot))
            {
                Assert.IsNotNull(transaction);
                Assert.AreEqual(connection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.Snapshot, transaction.IsolationLevel);
                transaction.Rollback();
            }
        }
    }
}
