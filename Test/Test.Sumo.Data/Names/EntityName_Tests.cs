using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sumo.Data
{
    [TestClass]
    public class EntityName_Tests
    {
        #region constructors
        [TestMethod]
        public void Constructor_Name()
        {
            var name = "name";
            var tableName = new EntityName(name);
            Assert.AreEqual(name, tableName.Name);
        }

        [TestMethod]
        public void Constructor_SchemaName()
        {
            var schema = "schema";
            var name = "name";
            var tableName = new EntityName(schema, name);
            Assert.AreEqual(name, tableName.Name);
            Assert.AreEqual(schema, tableName.Schema);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NameNullArgument()
        {
            string name = null;
            var tableName = new EntityName(name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NameStringEmptyArgument()
        {
            string name = String.Empty;
            var tableName = new EntityName(name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SchemaNullArgument()
        {
            string schema = null;
            var name = "name";
            var tableName = new EntityName(schema, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SchemaStringEmptyArgument()
        {
            string schema = String.Empty;
            var name = "name";
            var tableName = new EntityName(schema, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SchemaProvided_NameNullArgument()
        {
            string schema = "schema";
            string name = null;
            var tableName = new EntityName(schema, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SchemaProvided_NameStringEmptyArgument()
        {
            string schema = "schema";
            string name = String.Empty;
            var tableName = new EntityName(schema, name);
        }
        #endregion

        [TestMethod]
        public void InstanceToString_NameProvided()
        {
            var name = "name";
            EntityName tableName = new EntityName(name);
            Assert.AreEqual($"[{name}]", tableName.ToString());
        }

        [TestMethod]
        public void InterfaceToString_NameProvided()
        {
            var name = "name";
            IEntityName tableName = new EntityName(name);
            Assert.AreEqual($"[{name}]", tableName.ToString());
        }

        [TestMethod]
        public void InstanceToString_SchemaAndNameProvided()
        {
            var schema = "schema";
            var name = "name";
            EntityName tableName = new EntityName(schema, name);
            Assert.AreEqual($"[{schema}].[{name}]", tableName.ToString());
        }

        [TestMethod]
        public void InterfaceToString_SchemaAndNameProvided()
        {
            var schema = "schema";
            var name = "name";
            EntityName tableName = new EntityName(schema, name);
            Assert.AreEqual($"[{schema}].[{name}]", tableName.ToString());
        }

        [TestMethod]
        public void ImplicitStringOperator_NameProvided()
        {
            var name = "name";
            EntityName tableName = new EntityName(name);
            string tableNameString = tableName;
            Assert.AreEqual($"[{name}]", tableNameString);
        }

        [TestMethod]
        public void ImplicitStringOperator_SchemaAndNameProvided()
        {
            var schema = "schema";
            var name = "name";
            EntityName tableName = new EntityName(schema, name);
            string tableNameString = tableName;
            Assert.AreEqual($"[{schema}].[{name}]", tableNameString);
        }
    }
}