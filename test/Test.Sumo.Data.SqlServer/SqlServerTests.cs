using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sumo.Data.SqlServer
{
    // todo: create stored procedure tests until the correct folder
    // todo: move tests from this unit into appropriate units and delete this unit

    /// <summary>
    /// stored procedure is in the Test schema and is named Get
    /// sql would look like this
    /// exec [Test].[Get] @Id, @Out;
    /// </summary>
    [EntityPrefix("Test")]
    public sealed class Get
    {
        public long Id { get; set; } = 1;
        [OutputParameter]
        public int Status { get; set; }
    }

    public sealed class ReadParams
    {
        public long TestId { get; } = 1;
    }

    public class Test
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
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

            // on using transactions
            // https://stackoverflow.com/questions/6418992/is-it-a-better-practice-to-explicitly-call-transaction-rollback-or-let-an-except
            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var transaction = connection.BeginTransaction())
            using (var proc = new ReadProcedure(connection, parameterFactory, adapterFactory))
            {
                Assert.IsNotNull(proc);
                var getParams = new Get
                {
                    Id = 1
                };
                var readResult = proc.Read(getParams, transaction);

                Assert.AreEqual(-1, readResult.ReturnValue);
                Assert.AreEqual(1, readResult.DataSet.Tables.Count);
                Assert.AreEqual(1, readResult.DataSet.Tables[0].Rows.Count);
                Assert.AreEqual(1, getParams.Status);
                Assert.AreEqual(1, readResult.DataSet.Tables[0].Rows[0]["Id"]);
                Assert.AreEqual("one", readResult.DataSet.Tables[0].Rows[0]["Name"]);

                transaction.Commit();
            }
        }

        [TestMethod]
        public void SqlServerConnectionFactory_ReadProcedure_GetObjects()
        {
            var parameterFactory = new SqlServerParameterFactory();
            var adapterFactory = new SqlServerDataAdapterFactory();
            var connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var transaction = connection.BeginTransaction())
            using (var proc = new ReadProcedure(connection, parameterFactory, adapterFactory))
            {
                var getParams = new Get
                {
                    Id = 1
                };
                var readResult = proc.Read(getParams, transaction);
                var tests = readResult.DataSet.Tables[0].Rows.ToArray<Test>();
                Assert.IsNotNull(tests);
                Assert.AreEqual(1, tests.Length);
                var test = tests.FirstOrDefault();
                Assert.IsNotNull(test);
                Assert.AreEqual(1, test.Id);
                Assert.AreEqual("one", test.Name);

                transaction.Commit();
            }
        }
    }
}
