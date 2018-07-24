using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Factories;
using Sumo.Data.Factories.SqlServer;
using Sumo.Data.SqlServer.Tests.Application;
using Sumo.Retry;
using System;
using System.Data;

namespace Sumo.Data.SqlServer.Tests.Factories
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
            var retryOptions = new RetryOptions(10, TimeSpan.FromSeconds(60));

            ITransactionFactory transactionFactory = new SqlServerTransactionFactoryWithRetry(retryOptions);
            IConnectionFactory connectionFactory = new SqlServerConnectionFactoryWithRetry(retryOptions);
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
            var retryOptions = new RetryOptions(10, TimeSpan.FromSeconds(60));

            ITransactionFactory transactionFactory = new SqlServerTransactionFactoryWithRetry(retryOptions);
            IConnectionFactory connectionFactory = new SqlServerConnectionFactoryWithRetry(retryOptions);
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
