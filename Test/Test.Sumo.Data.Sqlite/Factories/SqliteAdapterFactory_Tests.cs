using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Factories;
using Sumo.Data.Factories.Sqlite;

namespace Sumo.Data.Sqlite.Factories
{
    [TestClass]
    public class SqliteAdapterFactory_Tests
    {
        string _connectionString = "Filename=./sqlite.db; Mode=ReadWriteCreate";

        [TestMethod]
        public void CreateDataAdapter()
        {
            Assert.IsTrue(_connectionString.CompareTo(_connectionString.ToUTF8()) == 0);
            
            IDataAdapterFactory dataAdapterFactory = new SqliteDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();
            using (var connection = connectionFactory.Open(_connectionString))
            using(var command = connection.CreateCommand())
            using (var dataAdapter = dataAdapterFactory.CreateDataAdapter(command))
            {
                Assert.IsNotNull(dataAdapter);
                Assert.AreEqual(command, dataAdapter.SelectCommand);
            }
        }
    }
}
