using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Test.Sumo.Data.Schema.Properties;

namespace Sumo.Data.Schema
{
    [TestClass]
    public class CatalogTest
    {
        private readonly string _owner = "dbo";

        public enum Color
        {
            Blue,
            Brown,
            Green,
            Gray
        }

        [Serializable]
        public class Person : IEquatable<Person>
        {
            public string Name { get; set; }
            public DateTime Dob { get; set; }
            public Color EyeColor { get; set; }

            #region IEquatable and Equals override

            public override bool Equals(object obj)
            {
                var person = obj as Person;
                return person != null &&
                       Name == person.Name &&
                       Dob == person.Dob &&
                       EyeColor == person.EyeColor;
            }

            public bool Equals(Person person)
            {
                return person != null &&
                       Name == person.Name &&
                       Dob == person.Dob &&
                       EyeColor == person.EyeColor;
            }

            public override int GetHashCode()
            {
                var hashCode = 907988741;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
                hashCode = hashCode * -1521134295 + Dob.GetHashCode();
                hashCode = hashCode * -1521134295 + EyeColor.GetHashCode();
                return hashCode;
            }

            public static bool operator ==(Person person1, Person person2)
            {
                return EqualityComparer<Person>.Default.Equals(person1, person2);
            }

            public static bool operator !=(Person person1, Person person2)
            {
                return !(person1 == person2);
            }
            #endregion
        }

        [Serializable]
        public class Employer
        {
            public string Name { get; set; }
            public List<Person> Employees { get; set; }
        }

        /// <summary>
        /// testing general binary serialization - basically a learning scratch pad
        /// </summary>
        [TestMethod]
        public void Binary()
        {
            var dude = new Person
            {
                Name = "Jimmy",
                Dob = new DateTime(1920, 12, 1),
                EyeColor = Color.Brown
            };

            var gal = new Person
            {
                Name = "Sally",
                Dob = new DateTime(1927, 5, 6),
                EyeColor = Color.Gray
            };

            var company = new Employer
            {
                Name = "Sumo Software Corporation",
                Employees = new List<Person> { dude, gal }
            };

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, company);
                stream.Flush();
                stream.Position = 0;
                var bytes = stream.ToArray();

                var companyCopy = (Employer)formatter.Deserialize(stream);
                Assert.AreEqual(company.Name, companyCopy.Name);
                Assert.AreEqual(2, companyCopy.Employees.Count);
                Assert.AreEqual(dude, companyCopy.Employees[0]);
            }
        }

        [TestMethod]
        public void Entity_FromJson()
        {
            var json = Resources.CatalogJson;
            var cat = json.FromJson<Catalog>();
        }

        [TestMethod]
        public void Entity_ToJson()
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

        [TestMethod]
        public void Entity_ToBytes()
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

            var bytes1 = catalog.ToBytes();
            var cat = bytes1.FromBytes<Catalog>();
            var bytes2 = cat.ToBytes();
            Assert.IsTrue(bytes1.SequenceEqual(bytes2));
        }

        [TestMethod]
        public void Entity_ToStream()
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

            using (var stream1 = catalog.ToStream())
            {
                var cat = stream1.FromStream<Catalog>();
                using (var stream2 = cat.ToStream())
                {
                    stream1.Position = 0;
                    stream2.Position = 0;

                    Assert.AreEqual(stream1.Length, stream2.Length);

                    var bytes1 = ((MemoryStream)stream1).ToArray();
                    var bytes2 = ((MemoryStream)stream2).ToArray();
                    Assert.IsTrue(bytes1.SequenceEqual(bytes2));
                }
            }            
        }
    }
}

