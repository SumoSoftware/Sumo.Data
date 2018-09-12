using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sumo.Data.SqlServer
{
    [TestClass]
    public class SqlServerAdapterFactory_Tests
    {
        [TestMethod]
        public void CreateDataAdapter()
        {
            IDataAdapterFactory dataAdapterFactory = new SqlServerDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();
            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using(var command = connection.CreateCommand())
            using (var dataAdapter = dataAdapterFactory.CreateDataAdapter(command))
            {
                Assert.IsNotNull(dataAdapter);
                Assert.AreEqual(command, dataAdapter.SelectCommand);
            }
        }
    }
}
