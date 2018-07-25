using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Factories.SqlServer;
using Sumo.Data.SqlServer.Application;

namespace Sumo.Data.SqlServer
{
    [TestClass]
    public class ScratchPad
    {
        [TestMethod]
        public void Command_CreateParamsFromSqlText()
        {
            //string sql = "select * from [tb] where [tb].[col] = @ParamOne";

            var adapterFactory = new SqlServerDataAdapterFactory();
            var connectionFactory = new SqlServerConnectionFactory();
            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var command = connection.CreateCommand())
            using (var adapter = adapterFactory.CreateDataAdapter(command))
            {
                Assert.IsNotNull(adapter);
                Assert.AreEqual(command, adapter.SelectCommand);
            }
        }
    }
}
