using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.SqlServer.Application;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Sumo.Data.SqlServer.Factories
{
    [TestClass]
    public class SqlServerConnectionFactory_Tests
    {
        [TestMethod]
        public void Open()
        {
            IConnectionFactory factory = new SqlServerConnectionFactory();
            using (var connection = factory.Open(AppState.ConnectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }

        [TestMethod]
        public async Task OpenAsync()
        {
            IConnectionFactory factory = new SqlServerConnectionFactory();
            using (var connection = await factory.OpenAsync(AppState.ConnectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }

        [TestMethod]
        public void Open_WithRetry()
        {
            IConnectionFactory factory = new SqlServerConnectionFactoryWithRetry(10, TimeSpan.FromSeconds(30));
            using (var connection = factory.Open(AppState.ConnectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }

        [TestMethod]
        public async Task OpenAsync_WithRetry()
        {
            IConnectionFactory factory = new SqlServerConnectionFactoryWithRetry(10, TimeSpan.FromSeconds(30));
            using (var connection = await factory.OpenAsync(AppState.ConnectionString))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(ConnectionState.Open, connection.State);
                connection.Close();
            }
        }
    }
}
