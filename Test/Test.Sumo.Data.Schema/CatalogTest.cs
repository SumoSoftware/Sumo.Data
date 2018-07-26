using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Expressions;
using System.Data;
using Test.Sumo.Data.Schema.Properties;

namespace Sumo.Data.Schema
{
    [TestClass]
    public class CatalogTest
    {
        private readonly string _owner = "dbo";

        [TestMethod]
        public void CreateCatalogFromJson()
        {
            var json = Resources.CatalogJson;
            var cat = json.FromJson<Catalog>();
        }

        [TestMethod]
        public void CreateCatalogJson()
        {
            var catalog = new Catalog("company_catalog", _owner);
            var schema = catalog.AddSchema(_owner);

            var jobTable = schema.AddTable("job");
            var jobPkColumn = jobTable.AddColumn("id", DbType.Int32);
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

            var json1 = catalog.ToJson();
            var cat = json1.FromJson<Catalog>();
            var json2 = cat.ToJson();
            Assert.AreEqual(json1, json2);
        }
    }
}
