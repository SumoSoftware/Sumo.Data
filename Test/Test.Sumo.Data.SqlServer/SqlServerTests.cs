using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Attributes;
using Sumo.Data.Factories.SqlServer;
using Sumo.Data.Orm;
using Sumo.Data.Procedures;
using Sumo.Data.SqlServer.Application;
using System.Linq;

namespace Sumo.Data.SqlServer
{
    [EntityPrefix("Test")]
    public sealed class Get
    {
        public long Id { get; } = 1;
        [OutputParameter]
        public int Out { get; set; }
    }

    public sealed class ReadParams
    {
        public long TestId { get; } = 1;
    }

    public class Test
    {
        public int TestId { get; set; }
        public string Name { get; set; }
    }

    [TestClass]
    public class SqlServerTests
    {
        [TestMethod]
        public void SqlServerConnectionFactory_ReadProcedure()
        {
            var parameterFactory = new SqlServerParameterFactory();
            var adapterFactory = new SqlServerDataAdapterFactory();
            var connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var proc = new ReadProcedure(connection, parameterFactory, adapterFactory))
            {
                Assert.IsNotNull(proc);
                var getParams = new Get();
                var readResult = proc.Read(getParams);

                Assert.AreEqual(99, readResult.ReturnValue);
                Assert.AreEqual(1, readResult.DataSet.Tables.Count);
                Assert.AreEqual(1, readResult.DataSet.Tables[0].Rows.Count);
                Assert.AreEqual(33, getParams.Out);
                Assert.AreEqual(1, readResult.DataSet.Tables[0].Rows[0]["TestId"]);
                Assert.AreEqual("one", readResult.DataSet.Tables[0].Rows[0]["Name"]);
            }
        }

        [TestMethod]
        public void SqlServerConnectionFactory_ReadProcedure_GetObjects()
        {
            var parameterFactory = new SqlServerParameterFactory();
            var adapterFactory = new SqlServerDataAdapterFactory();
            var connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var proc = new ReadProcedure(connection, parameterFactory, adapterFactory))
            {
                var getParams = new Get();
                var readResult = proc.Read(getParams);
                var tests = readResult.DataSet.Tables[0].Rows.ToArray<Test>();
                Assert.IsNotNull(tests);
                Assert.AreEqual(1, tests.Length);
                var test = tests.FirstOrDefault();
                Assert.IsNotNull(test);
                Assert.AreEqual(1, test.TestId);
                Assert.AreEqual("one", test.Name);
            }
        }
    }
}
