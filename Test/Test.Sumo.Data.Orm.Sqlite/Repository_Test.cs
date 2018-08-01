using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Factories.Sqlite;
using Sumo.Data.Orm.Factories;
using Sumo.Data.Attributes;
using Sumo.Data.Orm.Repositories;
using Sumo.Data.Schema.Factories.Sqlite;
using Sumo.Retry;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sumo.Data.Schema.Attributes;

namespace Test.Sumo.Data.Orm.Sqlite
{
    [TestClass]
    public class Repository_Test
    {
        //[AssemblyInitialize]
        //public static void ClassInitialize(TestContext testContext)
        //{
        //}

        private IFactorySet GetFactorySet()
        {
            var retryOptions = new RetryOptions(10, TimeSpan.FromSeconds(60));

            var parameterFactory = new SqliteSchemaParameterFactory();

            return new FactorySet(
                new SqliteConnectionFactoryWithRetry(retryOptions),
                new SqliteDataAdapterFactory(),
                parameterFactory,
                new SqliteTransactionFactoryWithRetry(retryOptions),
                new SqliteScriptBuilder(),
                new SqliteStatementBuilder(parameterFactory));
        }

        public class Person
        {
            [PrimaryKey]
            public long Id { get; set; } = -1;

            public int Age { get; set; }

            [Required, MaxSize(256)]
            public string FirstName { get; set; }

            [Required, MaxSize(256)]
            public string LastName { get; set; }
        }

        [TestMethod]
        public void Write()
        {
            var fileName = $"{nameof(Write)}.db";
            var _connectionString = $"Filename=./{fileName}; Mode=ReadWriteCreate";
            try
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
            finally
            {
                File.Delete($@".\{fileName}");
            }
        }

        [TestMethod]
        public async Task WriteAsync()
        {
            var fileName = $"{nameof(WriteAsync)}.db";
            var _connectionString = $"Filename=./{fileName}; Mode=ReadWriteCreate";
            try
            {
                var factorySet = GetFactorySet();
                IRepository repository = new Repository(factorySet, _connectionString);

                var person = new Person
                {
                    Age = 10,
                    FirstName = "Bobby",
                    LastName = "Jones"
                };

                await repository.WriteAsync(person);
            }
            finally
            {
                File.Delete($@".\{fileName}");
            }
        }

        [TestMethod]
        public void Read()
        {
            var fileName = $"{nameof(Read)}.db";
            File.Delete($@".\{fileName}");
            var _connectionString = $"Filename=./{fileName}; Mode=ReadWriteCreate";
            try
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

                var persons = repository.Read<Person>(new Dictionary<string, object> { ["LastName"] = "Jones" });
            }
            finally
            {
                File.Delete($@".\{fileName}");
            }
        }
    }
}
