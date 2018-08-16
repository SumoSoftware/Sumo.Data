using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sumo.Data
{
    [TestClass]
    public class ColumnName_Tests
    {
        #region constructors
        [TestMethod]
        public void Constructor_AliasAndNameProvided()
        {
            var alias = "alias";
            var name = "name";
            var aliasedColumnName = new ColumnName(name, alias);
            Assert.AreEqual(name, aliasedColumnName.Name);
            Assert.AreEqual(alias, aliasedColumnName.Alias);
        }

        [TestMethod]
        public void Constructor_AliasNullArgument()
        {
            string alias = null;
            var name = "name";
            var aliasedColumnName = new ColumnName(name, alias);
            Assert.AreEqual($"[{name}]", aliasedColumnName.ToString());
        }

        [TestMethod]
        public void Constructor_AliasStringEmptyArgument()
        {
            string alias = string.Empty;
            var name = "name";
            var aliasedColumnName = new ColumnName(name, alias);
            Assert.AreEqual($"[{name}]", aliasedColumnName.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AliasProvided_NameNullArgument()
        {
            string alias = "alias";
            string name = null;
            var aliasedColumnName = new ColumnName(name, alias);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AliasProvided_NameStringEmptyArgument()
        {
            string alias = "alias";
            string name = string.Empty;
            var aliasedColumnName = new ColumnName(name, alias);
        }
        #endregion

        [TestMethod]
        public void InstanceToString_AliasAndNameProvided()
        {
            var alias = "alias";
            var name = "name";
            ColumnName aliasedColumnName = new ColumnName(name, alias);
            Assert.AreEqual($"[{name}] as [{alias}]", aliasedColumnName.ToString());
        }

        [TestMethod]
        public void InterfaceToString_AliasAndNameProvided()
        {
            var alias = "alias";
            var name = "name";
            ColumnName aliasedColumnName = new ColumnName(name, alias);
            Assert.AreEqual($"[{name}] as [{alias}]", aliasedColumnName.ToString());
        }

        [TestMethod]
        public void ImplicitStringOperator_AliasAndNameProvided()
        {
            var alias = "alias";
            var name = "name";
            ColumnName aliasedColumnName = new ColumnName(name, alias);
            string aliasedColumnNameString = aliasedColumnName;
            Assert.AreEqual($"[{name}] as [{alias}]", aliasedColumnNameString);
        }
    }
}
