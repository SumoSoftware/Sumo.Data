using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Expressions;
using Sumo.Data.SqlExpressions;
using System;

namespace Sumo.Data
{
    [TestClass]
    public class JoinColumn_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            EntityName tableName = "tableName";
            ItemName columnName = "columnName";
            JoinColumn joinColumn = new JoinColumn(tableName, columnName);
            Assert.AreEqual(tableName, joinColumn.TableName);
            Assert.AreEqual(columnName, joinColumn.ColumnName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullTableName()
        {
            ItemName columnName = "columnName";
            JoinColumn joinColumn = new JoinColumn(null, columnName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullColumnName()
        {
            EntityName tableName = "tableName";
            JoinColumn joinColumn = new JoinColumn(tableName, null);
        }

        [TestMethod]
        public void ToStringTest()
        {
            EntityName tableName = "tableName";
            ItemName columnName = "columnName";
            JoinColumn joinColumn = new JoinColumn(tableName, columnName);
            Assert.AreEqual($"{tableName}.{columnName}", joinColumn.ToString());
        }

        [TestMethod]
        public void Equatable()
        {
            EntityName tableName = "tableName";
            ItemName columnName = "columnName";
            JoinColumn joinColumn1 = new JoinColumn(tableName, columnName);
            JoinColumn joinColumn2 = new JoinColumn(tableName, columnName);
            Assert.AreEqual(joinColumn1, joinColumn2);
            Assert.IsTrue(joinColumn1 == joinColumn2);
        }
    }

    [TestClass]
    public class JoinColumnMap_Tests
    {
        private JoinColumn GetJoinColumn(string table, string column)
        {
            EntityName tableName = table;
            ItemName columnName = column;
            return new JoinColumn(tableName, columnName);
        }

        [TestMethod]
        public void Constructor()
        {
            var leftColumn = GetJoinColumn("leftTable", "leftColumn");
            var rightColumn = GetJoinColumn("rightTable", "rightColumn");
            var map = new JoinColumnMap(leftColumn, rightColumn);
            Assert.AreEqual(RelationalOperators.Equal, map.RelationalOperator);
            Assert.AreEqual(LogicalOperators.And, map.LogicalOperator);
            Assert.AreEqual(leftColumn, map.LeftColumn);
            Assert.AreEqual(rightColumn, map.RightColumn);
        }

        [TestMethod]
        public void Equatable()
        {
            var leftColumn = GetJoinColumn("leftTable", "leftColumn");
            var rightColumn = GetJoinColumn("rightTable", "rightColumn");
            var map1 = new JoinColumnMap(leftColumn, rightColumn);

            var leftColumn2 = GetJoinColumn("leftTable", "leftColumn");
            var rightColumn2 = GetJoinColumn("rightTable", "rightColumn");
            var map2 = new JoinColumnMap(leftColumn2, rightColumn2);

            Assert.AreEqual(map1, map2);
            Assert.IsTrue(map1 == map2);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var leftColumn = GetJoinColumn("leftTable", "leftColumn");
            var rightColumn = GetJoinColumn("rightTable", "rightColumn");
            var map = new JoinColumnMap(leftColumn, rightColumn);
            var testString = $"{leftColumn}{RelationalOperators.Equal.ToSqlString()}{rightColumn}";
            Assert.AreEqual(testString, map.ToString());
        }
    }
}
