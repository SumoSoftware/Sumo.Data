using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.SqlExpressions;
using System;

namespace Sumo.Data
{
    [TestClass]
    public class FromTable_Tests
    {
        [TestMethod]
        public void NotFinished()
        {
            Assert.Fail($"todo: {GetType().FullName} - add ToString tests inclusive of expressions");
        }

        [TestMethod]
        public void Constructor()
        {
            IEntityName tableName = new EntityName("tableName");
            IItemName[] selectColumns = new IItemName[1] { new ColumnName("name", "alias") };
            var table = new FromTable(tableName, selectColumns);
            Assert.AreEqual(tableName, table.TableName);
            Assert.AreEqual(selectColumns, table.Columns);
            Assert.IsNull(table.JoinTables);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullTableNameArgument()
        {
            IItemName[] selectColumns = new IItemName[1] { new ColumnName("name", "alias") };
            var table = new FromTable(null, selectColumns);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullColumnsArgument()
        {
            IEntityName tableName = new EntityName("tableName");
            var table = new FromTable(tableName, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyColumnsArgument()
        {
            IEntityName tableName = new EntityName("tableName");
            IItemName[] selectColumns = new IItemName[0];

            var table = new FromTable(tableName, selectColumns);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_OneNullColumnArgument()
        {
            IEntityName tableName = new EntityName("tableName");
            IItemName[] selectColumns = new IItemName[2] { new ColumnName("name", "alias"), null };

            var table = new FromTable(tableName, selectColumns);
        }

        [TestMethod]
        public void ToString_WithoutSchema()
        {
            var name = "fromTable";
            var tableName = new EntityName(name);

            var fromColumn1 = "fromCol1";
            var fromColumn2 = "fromCol2";
            var fromAlias2 = "fromAlias2";
            var columns = new IItemName[]  {
                new ItemName(fromColumn1),
                new ColumnName(fromColumn2, fromAlias2)
            };
            var ft = new FromTable(tableName, columns);

            Assert.AreEqual($"select [{name}].[{fromColumn1}], [{name}].[{fromColumn2}] as [{fromAlias2}]\r\nfrom [{name}]\r\n", ft.ToString());
        }

        [TestMethod]
        public void ToString_WithSchema()
        {
            var schema = "fromSchema";
            var name = "fromTable";
            var tableName = new EntityName(schema, name);

            var fromColumn1 = "fromCol1";
            var fromColumn2 = "fromCol2";
            var fromAlias2 = "fromAlias2";
            var columns = new IItemName[]  {
                new ItemName(fromColumn1),
                new ColumnName(fromColumn2, fromAlias2)
            };
            var ft = new FromTable(tableName, columns);

            Assert.AreEqual($"select [{schema}].[{name}].[{fromColumn1}], [{schema}].[{name}].[{fromColumn2}] as [{fromAlias2}]\r\nfrom [{schema}].[{name}]\r\n", ft.ToString());
        }

        [TestMethod]
        public void ToString_WithSchemaAndOneColumn()
        {
            var schema = "fromSchema";
            var name = "fromTable";
            var tableName = new EntityName(schema, name);

            var fromColumn1 = "fromCol1";
            var columns = new IItemName[] { new ItemName(fromColumn1) };
            var ft = new FromTable(tableName, columns);

            Assert.AreEqual($"select [{schema}].[{name}].[{fromColumn1}]\r\nfrom [{schema}].[{name}]\r\n", ft.ToString());
        }

        [TestMethod]
        public void ToString_WithSchema_AsInterface()
        {
            var schema = "fromSchema";
            var name = "fromTable";
            var tableName = new EntityName(schema, name);

            var fromColumn1 = "fromCol1";
            var fromColumn2 = "fromCol2";
            var fromAlias2 = "fromAlias2";
            var columns = new IItemName[]  {
                new ItemName(fromColumn1),
                new ColumnName(fromColumn2, fromAlias2)
            };
            IFromTable ft = new FromTable(tableName, columns);

            Assert.AreEqual($"select [{schema}].[{name}].[{fromColumn1}], [{schema}].[{name}].[{fromColumn2}] as [{fromAlias2}]\r\nfrom [{schema}].[{name}]\r\n", ft.ToString());
        }

        [TestMethod]
        public void ToString_WithJoinTable_NoJTSelectColumns()
        {
            var schema = "fromSchema";
            var name = "fromTable";
            var tableName = new EntityName(schema, name);

            var fromColumn1 = "fromCol1";
            var fromColumn2 = "fromCol2";
            var fromAlias2 = "fromAlias2";
            var columns = new IColumnName[]  {
                new ColumnName(fromColumn1),
                new ColumnName(fromColumn2, fromAlias2)
            };
            IFromTable fromTable = new FromTable(tableName, columns);

            IEntityName joinTableName = new EntityName("joinTable");
            var joinTable = new JoinTable(joinTableName);

            fromTable.Join(joinTable);

            var selectClause = fromTable.ToString();
            var testString = $"select [{schema}].[{name}].[{fromColumn1}], [{schema}].[{name}].[{fromColumn2}] as [{fromAlias2}]\r\nfrom [{schema}].[{name}]\r\n";
            Assert.AreEqual(testString.TrimEnd(), selectClause.TrimEnd());
        }

        [TestMethod]
        public void ToString_WithJoinTable()
        {
            var schema = "fromSchema";
            var name = "fromTable";
            var tableName = new EntityName(schema, name);

            var fromColumn1 = "fromCol1";
            var fromColumn2 = "fromCol2";
            var fromAlias2 = "fromAlias2";
            var columns = new IItemName[]  {
                new ItemName(fromColumn1),
                new ColumnName(fromColumn2, fromAlias2)
            };
            var fromTable = new FromTable(tableName, columns);

            IEntityName joinTableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[2] { new ItemName("joinTblCol1"), new ItemName("joinTblCol2") };
            var joinTable = new JoinTable(joinTableName, selectColumns);

            fromTable.Join(joinTable);

            var selectClause = fromTable.GetSelectClause();
            var testString = $"[{schema}].[{name}].[{fromColumn1}], [{schema}].[{name}].[{fromColumn2}] as [{fromAlias2}], {joinTableName}.{selectColumns[0]}, {joinTableName}.{selectColumns[1]}";
            Assert.AreEqual(testString, selectClause);
        }

        [TestMethod]
        public void ToString_WithJoinTableAndOneColumn()
        {
            var schema = "fromSchema";
            var name = "fromTable";
            var tableName = new EntityName(schema, name);

            var fromColumn1 = "fromCol1";
            var columns = new IItemName[] { new ItemName(fromColumn1) };
            var fromTable = new FromTable(tableName, columns);

            IEntityName joinTableName = new EntityName("joinTable");
            IItemName[] selectColumns = new IItemName[2] { new ItemName("joinTblCol1"), new ItemName("joinTblCol2") };
            var joinTable = new JoinTable(joinTableName, selectColumns);

            fromTable.Join(joinTable);

            var selectClause = fromTable.GetSelectClause();
            var testString = $"[{schema}].[{name}].[{fromColumn1}], {joinTableName}.{selectColumns[0]}, {joinTableName}.{selectColumns[1]}";
            Assert.AreEqual(testString, selectClause);
        }
    }
}