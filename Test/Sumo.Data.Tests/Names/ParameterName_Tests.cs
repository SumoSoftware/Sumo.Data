using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sumo.Data.Names
{
    [TestClass]
    public class ParameterName_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var name = "name";
            var parameterName = new ParameterName(name);
            Assert.AreEqual(name, parameterName.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullArgument()
        {
            string name = null;
            var parameterName = new ParameterName(name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_StringEmptyArgument()
        {
            string name = string.Empty;
            var parameterName = new ParameterName(name);
        }

        [TestMethod]
        public void InstanceToString()
        {
            var name = "name";
            var parameterName = new ParameterName(name);
            Assert.AreEqual($"@{name}", parameterName.ToString());
        }

        [TestMethod]
        public void InterfaceToString()
        {
            var name = "name";
            IParameterName parameterName = new ParameterName(name);
            Assert.AreEqual($"@{name}", parameterName.ToString());
        }

        [TestMethod]
        public void ImplicitStringOperator()
        {
            var name = "name";
            var parameterName = new ParameterName(name);
            string parameterNameString = parameterName;
            Assert.AreEqual($"@{name}", parameterNameString);
        }
    }
}
