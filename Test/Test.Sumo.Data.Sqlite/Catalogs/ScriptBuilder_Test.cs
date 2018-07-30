using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Commands;
using Sumo.Data.Expressions;
using Sumo.Data.Factories;
using Sumo.Data.Factories.Sqlite;
using Sumo.Data.Schema;
using Sumo.Data.Schema.Factories.Sqlite;
using System.Data;

namespace Sumo.Data.Sqlite.Catalogs
{
    [TestClass]
    public class ScriptBuilder_Test
    {
        private readonly string _schemaName = "main";

        [TestMethod]
        public void CreateCreateScript()
        {
            var catalog = new Catalog("company_catalog");
            var schema = catalog.AddSchema(_schemaName);

            var jobTable = schema.AddTable("job");
            var jobPkColumn = jobTable.AddColumn("id", DbType.Int64);
            jobPkColumn.IsPrimaryKey = true;
            jobPkColumn.PrimaryKey.IsAutoIncrement = true;

            var jobNameColumn = jobTable.AddColumn("name", DbType.String);
            jobNameColumn.MaxLength = 256;
            jobNameColumn.IsUnique = true;

            var employeeTable = schema.AddTable("employee");
            employeeTable.AddComment("employees and their date of employment");

            var pkColumn = employeeTable.AddColumn("id", DbType.Int32);
            pkColumn.IsPrimaryKey = true;
            pkColumn.PrimaryKey.IsAutoIncrement = true;

            var jobIdColumn = employeeTable.AddColumn("job_id", DbType.Int32);
            jobIdColumn.HasForeignKey = true;
            jobIdColumn.ForeignKey.Schema = schema.Name;
            jobIdColumn.ForeignKey.Table = jobTable.Name;
            jobIdColumn.ForeignKey.Column = jobPkColumn.Name;

            var firstNameColumn = employeeTable.AddColumn("first_name", DbType.String);
            firstNameColumn.MaxLength = 256;
            firstNameColumn.IsNullable = false;

            var lastNameColumn = employeeTable.AddColumn("last_name", DbType.String);
            lastNameColumn.MaxLength = 256;
            lastNameColumn.IsNullable = false;

            var ssnColumn = employeeTable.AddColumn("ssn", DbType.String);
            ssnColumn.MaxLength = 11;
            ssnColumn.IsNullable = false;

            var doeColumn = employeeTable.AddColumn("doe", DbType.Date);
            doeColumn.AddComment("date of employment");
            doeColumn.IsNullable = false;

            var dobColumn = employeeTable.AddColumn("dob", DbType.Date);

            var index = employeeTable.AddIndex("employee_name_idx");
            index.AddColumn(lastNameColumn, Directions.Ascending);
            index.AddColumn(firstNameColumn);
            index.AddColumn(ssnColumn);
            index.IsUnique = true;

            var builder = new SqliteScriptBuilder();
            var sql = builder.BuildDbCreateScript(catalog);
        }

        [TestMethod]
        public void CreateCatalog()
        {
            var connectionString = "Filename=./sqlite.db; Mode=ReadWriteCreate";

            var catalog = new Catalog("test_db");
            var schema = catalog.AddSchema(_schemaName);
            var table = schema.AddTable("Test");
            var colPk = table.AddColumn("TestId", DbType.Int64);
            colPk.IsPrimaryKey = true;
            var colName = table.AddColumn("Name", DbType.String);
            colName.MaxLength = 256;
            colName.IsNullable = false;

            var builder = new SqliteScriptBuilder();
            var sql = builder.BuildDbCreateScript(catalog);

            IParameterFactory parameterFactory = new SqliteParameterFactory();
            IConnectionFactory connectionFactory = new SqliteConnectionFactory();
            using (var connection = connectionFactory.Open(connectionString))
            using (var command = new Command(connection, parameterFactory))
            {
                var affected = command.Execute(sql);
                //Assert.IsNotNull(dataSet);
                //Assert.AreEqual(1, dataSet.Tables.Count);
                //Assert.IsTrue(dataSet.Tables[0].Rows.Count > 0);
            }
        }
    }
}
