using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sumo.Data
{
    [TestClass]
    public class ParameterName_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var name = "name";
            var parameterName = new ParameterName(name, -1);
            Assert.AreEqual(name, parameterName.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullArgument()
        {
            string name = null;
            var parameterName = new ParameterName(name, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_StringEmptyArgument()
        {
            string name = String.Empty;
            var parameterName = new ParameterName(name, -1);
        }

        [TestMethod]
        public void InstanceToString()
        {
            var name = "name";
            var parameterName = new ParameterName(name,-1);
            Assert.AreEqual($"@{name}", parameterName.ToString());
        }

        [TestMethod]
        public void InterfaceToString()
        {
            var name = "name";
            IParameterName parameterName = new ParameterName(name, -1);
            Assert.AreEqual($"@{name}", parameterName.ToString());
        }

        [TestMethod]
        public void ImplicitStringOperator()
        {
            var name = "name";
            var parameterName = new ParameterName(name,-1);
            string parameterNameString = parameterName;
            Assert.AreEqual($"@{name}", parameterNameString);
        }
    }
}
