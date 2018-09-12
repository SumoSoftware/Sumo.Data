using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data;
using Sumo.Data.Schema;
using Sumo.Data.Schema.Sqlite;
using Sumo.Data.Sqlite;
using System.Threading.Tasks;

namespace Test.Sumo.Data.Sqlite.Commands
{
    public class Test
    {
        public int TestId { get; set; }
        public string Name { get; set; }
    }

    public class DoesNotExist
    {

    }

    [TestClass]
    public class Command_Tests
    {
        private readonly string _connectionString = "Filename=./sqlite.db; Mode=ReadWriteCreate";

        [TestMethod]
        public void Constructor()
        {
            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();

            using (var connection = connectionFactory.Open(_connectionString))
            using (var command = new Command(connection, parameterFactory))
            {
                Assert.IsNotNull(command);
            }
        }

        [TestMethod]
        public void ExecuteScalor()
        {
            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();
            ISqlStatementBuilder sqlBuilder = new SqliteStatementBuilder(parameterFactory);

            using (var connection = connectionFactory.Open(_connectionString))
            using (var command = new Command(connection, parameterFactory))
            {
                var sql = sqlBuilder.GetExistsStatement<Test>();
                var exists = command.ExecuteScalar<bool>(sql);
                Assert.IsTrue(exists);

                sql = sqlBuilder.GetExistsStatement<DoesNotExist>();
                exists = command.ExecuteScalar<bool>(sql);
                Assert.IsFalse(exists);
            }
        }

        [TestMethod]
        public async Task ExecuteScalorAsync()
        {
            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();
            ISqlStatementBuilder sqlBuilder = new SqliteStatementBuilder(parameterFactory);

            using (var connection = connectionFactory.Open(_connectionString))
            using (var command = new Command(connection, parameterFactory))
            {
                var sql = sqlBuilder.GetExistsStatement<Test>();
                var exists = await command.ExecuteScalarAsync<bool>(sql);
                Assert.IsTrue(exists);

                sql = sqlBuilder.GetExistsStatement<DoesNotExist>();
                exists = await command.ExecuteScalarAsync<bool>(sql);
                Assert.IsFalse(exists);
            }
        }
    }
}
