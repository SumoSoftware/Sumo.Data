using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Factories;
using Sumo.Data.Factories.Sqlite;
using Sumo.Data.Queries;
using System.Collections.Generic;

namespace Sumo.Data.Sqlite.Tests.Queries
{
    [TestClass]
    public class SqliteSqlReader_Tests
    {
        private readonly string _connectionString = "Filename=./sqlite.db; Mode=ReadWriteCreate";

        [TestMethod]
        public void Constructor()
        {
            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqliteDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();

            using (var connection = connectionFactory.Open(_connectionString))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                Assert.IsNotNull(reader);
            }                 
        }

        [TestMethod]
        public void Read()
        {
            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqliteDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();

            using (var connection = connectionFactory.Open(_connectionString))
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
            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqliteDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();

            using (var connection = connectionFactory.Open(_connectionString))
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
            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IDataAdapterFactory dataAdapterFactory = new SqliteDataAdapterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();

            using (var connection = connectionFactory.Open(_connectionString))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                var dataSet = reader.Read("select * from Test where TestId=@TestId", new Dictionary<string, object> { ["TestId"] = 1 });
                Assert.IsNotNull(dataSet);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.AreEqual(1, dataSet.Tables[0].Rows.Count);
            }
        }
    }
}
