using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Orm.Sqlite;
using Sumo.Data.Schema;
using Sumo.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sumo.Data.Orm
{
    [TestClass]
    public class Repository_Test
    {
        //[AssemblyInitialize]
        //public static void ClassInitialize(TestContext testContext)
        //{
        //}

        private IOrmDataComponentFactory GetFactorySet()
        {
            var retryPolicy = new SqliteTransientRetryPolicy(10, TimeSpan.FromSeconds(60));
            return new SqliteOrmDataComponentFactory(retryPolicy);
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
                IRepository repository = new Repository(new SqliteOrmDataComponentFactory(_connectionString));

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
                IRepository repository = new Repository(new SqliteOrmDataComponentFactory(_connectionString));

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
                IRepository repository = new Repository(new SqliteOrmDataComponentFactory(_connectionString));

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
