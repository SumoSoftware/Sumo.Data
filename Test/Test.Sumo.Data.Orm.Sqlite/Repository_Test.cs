using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Retry;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sumo.Data.Schema;
using Sumo.Data.Schema.Sqlite;
using Sumo.Data.Sqlite;

namespace Sumo.Data.Orm
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

            var dataProviderFactory = new SqliteDataComponentFactory(retryOptions);
            var parameterFactory = new SqliteSchemaParameterNames();

            return new FactorySet(
                dataProviderFactory,
                parameterFactory,
                new SqliteScriptBuilder(),
                new SqliteStatementBuilder(dataProviderFactory));
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
