using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sumo.Data.SqlServer.Queries
{
    [TestClass]
    public class SqlServerSqlReader_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            IParameterFactory parameterFactory = new SqlServerParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqlServerDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                Assert.IsNotNull(reader);
            }
        }

        [TestMethod]
        public void Read()
        {
            IParameterFactory parameterFactory = new SqlServerParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqlServerDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                var dataSet = reader.Read("select * from Test");
                Assert.IsNotNull(dataSet);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.IsTrue(dataSet.Tables[0].Rows.Count > 0);
            }
        }

        [TestMethod]
        public void Read_Count()
        {
            IParameterFactory parameterFactory = new SqlServerParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqlServerDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                var dataSet = reader.Read("select count(*) from Test");
                Assert.IsNotNull(dataSet);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.IsTrue(dataSet.Tables[0].Rows.Count > 0);
            }
        }

        private sealed class ReadParams
        {
            public long TestId { get; } = 1;
        }

        [TestMethod]
        public void Read_WithParams()
        {
            IParameterFactory parameterFactory = new SqlServerParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqlServerDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                var dataSet = reader.Read("select * from Test where Status=@Status", new Dictionary<string, object> { ["Status"] = 1 });
                Assert.IsNotNull(dataSet);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.AreEqual(1, dataSet.Tables[0].Rows.Count);
            }
        }

        public enum Statuses : int
        {
            Zero,
            One,
            Two
        }

        public class Test
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Statuses Status { get; set; }
        }

        [TestMethod]
        public void Read_ToArray()
        {
            IParameterFactory parameterFactory = new SqlServerParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqlServerDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqlServerConnectionFactory();

            using (var connection = connectionFactory.Open(AppState.ConnectionString))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                //var x = new int[] { 1, 2 };
                //var dataSet = reader.Read($"select * from Test where Id in ({System.String.Join(",", x)})");
                var dataSet = reader.Read("select * from Test in");

                Assert.IsNotNull(dataSet);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.IsTrue(dataSet.Tables[0].Rows.Count > 0);

                var records = dataSet.Tables[0].Rows.ToArray<Test>();
                Assert.IsNotNull(records);
                Assert.IsTrue(records.Length > 0);
            }
        }
    }
}
