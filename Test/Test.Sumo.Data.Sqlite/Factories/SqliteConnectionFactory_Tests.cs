using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Sumo.Data.Sqlite
{
    [TestClass]
    public class SqliteConnectionFactory_Tests
    {
        private readonly string _connectionString = "Filename=./sqlite.db; Mode=ReadWriteCreate";

        [TestMethod]
        public void Open()
        {
            IConnectionFactory factory = new SqliteConnectionFactory();
            using (var connection = factory.Open(_connectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }

        [TestMethod]
        public async Task OpenAsync()
        {
            IConnectionFactory factory = new SqliteConnectionFactory();
            using (var connection = await factory.OpenAsync(_connectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }

        [TestMethod]
        public void Open_WithRetry()
        {
            IConnectionFactory factory = new SqliteConnectionFactoryWithRetry(10, TimeSpan.FromSeconds(30));
            using (var connection = factory.Open(_connectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }

        [TestMethod]
        public async Task OpenAsync_WithRetry()
        {
            IConnectionFactory factory = new SqliteConnectionFactoryWithRetry(10, TimeSpan.FromSeconds(30));
            using (var connection = await factory.OpenAsync(_connectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }
    }
}
