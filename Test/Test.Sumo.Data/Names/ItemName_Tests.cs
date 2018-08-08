using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sumo.Data.Names
{
    [TestClass]
    public class ItemName_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var name = "name";
            var itemName = new ItemName(name);
            Assert.AreEqual(name, itemName.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullArgument()
        {
            string name = null;
            var itemName = new ItemName(name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_StringEmptyArgument()
        {
            string name = String.Empty;
            var itemName = new ItemName(name);
        }

        [TestMethod]
        public void InstanceToString()
        {
            var name = "name";
            var itemName = new ItemName(name);
            Assert.AreEqual($"[{name}]", itemName.ToString());
        }

        [TestMethod]
        public void InterfaceToString()
        {
            var name = "name";
            IItemName itemName = new ItemName(name);
            Assert.AreEqual($"[{name}]", itemName.ToString());
        }

        [TestMethod]
        public void ImplicitStringOperator()
        {
            var name = "name";
            var itemName = new ItemName(name);
            string itemNameString = itemName;
            Assert.AreEqual($"[{name}]", itemNameString);
        }
    }
}
