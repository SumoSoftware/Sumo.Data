using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Factories.Sqlite;
using Sumo.Data.Orm.Factories;
using Sumo.Data.Attributes;
using Sumo.Data.Orm.Repositories;
using Sumo.Data.Schema.Factories.Sqlite;
using Sumo.Retry;
using System;

namespace Test.Sumo.Data.Orm.Sqlite
{
    [TestClass]
    public class Repository_Test
    {

        private readonly string _connectionString = "Filename=./sqlite.db; Mode=ReadWriteCreate";

        private IFactorySet GetFactorySet()
        {
            var retryOptions = new RetryOptions(10, TimeSpan.FromSeconds(60));

            return new FactorySet(
                new SqliteConnectionFactoryWithRetry(retryOptions),
                new SqliteDataAdapterFactory(),
                new SqliteParameterFactory(),
                new SqliteTransactionFactoryWithRetry(retryOptions),
                new SqliteScriptBuilder(),
                new SqliteStatementBuilder());
        }

        public class Person
        {
            [PrimaryKey]
            public int Id { get; set; } = -1;

            public int Age { get; set; }

            [Required, MaxSize(256)]
            public string FirstName { get; set; }

            [Required, MaxSize(256)]
            public string LastName { get; set; }
        }

        [TestMethod]
        public void Write()
        {
            var factorySet = GetFactorySet();
            IRepository repository = new Repository(factorySet, _connectionString);

            var person = new Person
            {
                Age = 10,
                FirstName = "Bobby",
                LastName = "Jones"
            };

            repository.Write(person);
        }
    }
}
