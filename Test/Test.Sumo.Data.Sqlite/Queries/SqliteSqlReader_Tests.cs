using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sumo.Data.Sqlite.Queries
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
            using (var command = new Command(connection, parameterFactory))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                command.Execute("drop table if exists ReadTest");
                command.Execute("create table if not exists ReadTest(TestColumn text)");
                command.Execute("insert into ReadTest values('one')");
                command.Execute("insert into ReadTest values('two')");

                var dataSet = reader.Read("select * from ReadTest");
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
            using (var command = new Command(connection, parameterFactory))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                command.Execute("drop table if exists ReadCountTest");
                command.Execute("create table if not exists ReadCountTest(TestColumn text)");
                command.Execute("insert into ReadCountTest values('one')");
                command.Execute("insert into ReadCountTest values('two')");

                var dataSet = reader.Read("select count(*) from ReadCountTest");
                Assert.IsNotNull(dataSet);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.AreEqual(1, dataSet.Tables[0].Rows.Count);
                Assert.AreEqual(2L, (long)dataSet.Tables[0].Rows[0][0]);
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
            using (var command = new Command(connection, parameterFactory))
            using (var reader = new SqlReader(connection, parameterFactory, dataAdapterFactory))
            {
                command.Execute("drop table if exists ReadWParamsTest");
                command.Execute("create table if not exists ReadWParamsTest(TestId integer primary key autoincrement, TestValue text)");
                command.Execute("insert into ReadWParamsTest(TestValue) values('one')");
                command.Execute("insert into ReadWParamsTest(TestValue) values('two')");

                var dataSet = reader.Read("select * from ReadWParamsTest where TestId=@TestId", new Dictionary<string, object> { ["TestId"] = 1 });
                Assert.IsNotNull(dataSet);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.AreEqual(1, dataSet.Tables[0].Rows.Count);
            }
        }
    }
}
