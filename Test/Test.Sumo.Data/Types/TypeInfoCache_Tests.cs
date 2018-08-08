using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Test.Support;
using System;

namespace Sumo.Data.Types
{
    [TestClass]
    public class TypeInfoCache_Tests
    {
        [TestMethod]
        public void FullName()
        {
            Assert.AreEqual(typeof(TestTypePrefixAndCustomName).FullName, TypeInfoCache<TestTypePrefixAndCustomName>.FullName);
        }

        [TestMethod]
        public void SerializableParameters()
        {
            Assert.AreEqual(4, TypeInfoCache<TestTypePrefixAndCustomName>.Properties.Length);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomName.ReadWriteProperty), TypeInfoCache<TestTypePrefixAndCustomName>.Properties[0].Name);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomName.ReadPrivateWriteProperty), TypeInfoCache<TestTypePrefixAndCustomName>.Properties[1].Name);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomName.ReadProperty), TypeInfoCache<TestTypePrefixAndCustomName>.Properties[2].Name);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomName.OutputParameter), TypeInfoCache<TestTypePrefixAndCustomName>.Properties[3].Name);
        }

        [TestMethod]
        public void TypeCodes()
        {
            Assert.AreEqual(TypeInfoCache<TestTypePrefixAndCustomName>.Properties.Length, TypeInfoCache<TestTypePrefixAndCustomName>.TypeCodes.Length);
            Assert.AreEqual(TypeCode.String, TypeInfoCache<TestTypePrefixAndCustomName>.TypeCodes[0]);
            Assert.AreEqual(TypeCode.Int32, TypeInfoCache<TestTypePrefixAndCustomName>.TypeCodes[3]);
        }

        [TestMethod]
        public void ReadOnlyPropertiesLinq()
        {
            Assert.AreEqual(2, TypeInfoCache<TestTypeReadOnlyProperties>.ReadOnlyProperties.Length);
            Assert.AreEqual(nameof(TestTypeReadOnlyProperties.PrivateSetProperty),
                TypeInfoCache<TestTypeReadOnlyProperties>.ReadOnlyProperties[0].Name);
            Assert.AreEqual(nameof(TestTypeReadOnlyProperties.GetOnlyProperty),
                TypeInfoCache<TestTypeReadOnlyProperties>.ReadOnlyProperties[1].Name);
        }
    }
}
