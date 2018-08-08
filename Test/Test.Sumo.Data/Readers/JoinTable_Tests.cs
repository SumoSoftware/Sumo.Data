using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Names;
using Sumo.Data.SqlExpressions;
using System;

namespace Sumo.Data.Readers
{
    [TestClass]
    public class JoinTable_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            IEntityName tableName = new EntityName("joinTable");
            var table = new JoinTable(tableName);
            Assert.AreEqual(tableName, table.TableName);
            Assert.IsNull(table.Columns);
        }

        [TestMethod]
        public void Constructor_WithColumns()
        {
            IEntityName tableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[1] { new ItemName("joinColumn") };
            var table = new JoinTable(tableName, selectColumns);
            Assert.AreEqual(tableName, table.TableName);
            Assert.AreEqual(selectColumns, table.Columns);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullTableNameArgument()
        {
            IItemName[] selectColumns = new IItemName[1] { new ItemName("joinColumn") };
            var table = new JoinTable(null, selectColumns);
        }

        [TestMethod]
        public void Constructor_NullColumnsArgument()
        {
            IEntityName tableName = new EntityName("joinTable");
            var table = new JoinTable(tableName, null);
            Assert.AreEqual(tableName, table.TableName);
            Assert.IsNull(table.Columns);
        }

        [TestMethod]
        public void Constructor_EmptyColumnsArgument()
        {
            IEntityName tableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[0];
            var table = new JoinTable(tableName, selectColumns);
            Assert.AreEqual(0, table.Columns.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_OneNullColumnArgument()
        {
            IEntityName tableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[2] { new ItemName("joinColumn"), null };
            var table = new JoinTable(tableName, selectColumns);
        }

        [TestMethod]
        public void AddJoinColumn()
        {
            IEntityName tableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[1] { new ItemName("joinColumn") };
            var table = new JoinTable(tableName, selectColumns);
            EntityName fromTableName = "fromTable";
            ItemName leftColumn = "leftJoinColumn";
            ItemName rightColumn = "rightJoinColumn";
            table.AddJoinColumn(fromTableName, leftColumn, rightColumn);
        }

        [TestMethod]
        public void ToString_NoJoinColumns()
        {
            IEntityName tableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[1] { new ItemName("selectColumn") };
            var table = new JoinTable(tableName, selectColumns);
            Assert.AreEqual(String.Empty, table.ToString());
        }

        [TestMethod]
        public void ToString_OneJoinColumn()
        {
            IEntityName tableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[1] { new ItemName("selectColumn") };
            var table = new JoinTable(tableName, selectColumns);
            EntityName fromTableName = "fromTable";
            ItemName leftColumn1 = "leftJoinColumn";
            ItemName rightColumn1 = "rightJoinColumn";
            table.AddJoinColumn(fromTableName, leftColumn1, rightColumn1);

            var testString = $"join {tableName} on {fromTableName}.{leftColumn1}{RelationalOperators.Equal.ToSqlString()}{table.TableName}.{rightColumn1}";
            Assert.AreEqual(testString, table.ToString());
        }

        [TestMethod]
        public void ToString_OneTwoColumns()
        {
            IEntityName tableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[1] { new ItemName("selectColumn") };
            var table = new JoinTable(tableName, selectColumns);
            EntityName fromTableName = "fromTable";
            ItemName leftColumn1 = "leftJoinColumn1";
            ItemName rightColumn1 = "rightJoinColumn1";
            table.AddJoinColumn(fromTableName, leftColumn1, rightColumn1);

            ItemName leftColumn2 = "leftJoinColumn2";
            ItemName rightColumn2 = "rightJoinColumn2";
            table.AddJoinColumn(fromTableName, leftColumn2, rightColumn2, RelationalOperators.GreaterThan, LogicalOperators.And);

            var testString = $"join {tableName} on {fromTableName}.{leftColumn1}{RelationalOperators.Equal.ToSqlString()}{table.TableName}.{rightColumn1} {LogicalOperators.And.ToSqlString()} {fromTableName}.{leftColumn2}{RelationalOperators.GreaterThan.ToSqlString()}{table.TableName}.{rightColumn2}";
            Assert.AreEqual(testString, table.ToString());
        }
    }
}
