using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Retry;
using System;
using System.Data;

namespace Sumo.Data.Sqlite
{
    [TestClass]
    public class TransactionFactory_Tests
    {
        private readonly string _connectionString = "Filename=./sqlite.db; Mode=ReadWriteCreate";

        [TestMethod]
        public void BeginTransaction()
        {
            ITransactionFactory transactionFactory = new TransactionFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();
            using (var connection = connectionFactory.Open(_connectionString))
            using(var transaction = transactionFactory.BeginTransaction(connection))
            {
                Assert.IsNotNull(transaction);
                Assert.AreEqual(connection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.Serializable, transaction.IsolationLevel);
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void BeginTransaction_WithIsolationLevel()
        {
            // sqlite only supports IsolationLevel.Serializable
            ITransactionFactory transactionFactory = new TransactionFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();
            using (var connection = connectionFactory.Open(_connectionString))
            using (var transaction = transactionFactory.BeginTransaction(connection, IsolationLevel.Serializable))
            {
                Assert.IsNotNull(transaction);
                Assert.AreEqual(connection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.Serializable, transaction.IsolationLevel);
                transaction.Rollback();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BeginTransaction_WithIsolationLevel_Failure()
        {
            // sqlite only supports IsolationLevel.Serializable
            ITransactionFactory transactionFactory = new TransactionFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();
            using (var connection = connectionFactory.Open(_connectionString))
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

            ITransactionFactory transactionFactory = new SqliteTransactionFactoryWithRetry(retryOptions);
            IConnectionFactory connectionFactory = new SqliteConnectionFactoryWithRetry(retryOptions);
            using (var connection = connectionFactory.Open(_connectionString))
            using (var transaction = transactionFactory.BeginTransaction(connection))
            {
                Assert.IsNotNull(transaction);
                Assert.AreEqual(connection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.Serializable, transaction.IsolationLevel);
                transaction.Rollback();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RetryNotAllowedException))]
        public void BeginTransaction_WithIsolationLevel_WithRetry_Fail()
        {
            // sqlite only supports IsolationLevel.Serializable
            var retryOptions = new RetryOptions(10, TimeSpan.FromSeconds(60));

            ITransactionFactory transactionFactory = new SqliteTransactionFactoryWithRetry(retryOptions);
            IConnectionFactory connectionFactory = new SqliteConnectionFactoryWithRetry(retryOptions);
            using (var connection = connectionFactory.Open(_connectionString))
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
