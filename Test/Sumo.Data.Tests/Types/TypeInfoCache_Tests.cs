using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Support;
using System;

namespace Sumo.Data.Types
{
    [TestClass]
    public class TypeInfoCache_Tests
    {
        [TestMethod]
        public void FullName()
        {
            Assert.AreEqual(typeof(TestTypeFullAppendix).FullName, TypeInfoCache<TestTypeFullAppendix>.FullName);
        }

        [TestMethod]
        public void SerializableParameters()
        {
            Assert.AreEqual(4, TypeInfoCache<TestTypeFullAppendix>.Properties.Length);
            Assert.AreEqual(nameof(TestTypeFullAppendix.ReadWriteProperty), TypeInfoCache<TestTypeFullAppendix>.Properties[0].Name);
            Assert.AreEqual(nameof(TestTypeFullAppendix.ReadPrivateWriteProperty), TypeInfoCache<TestTypeFullAppendix>.Properties[1].Name);
            Assert.AreEqual(nameof(TestTypeFullAppendix.ReadProperty), TypeInfoCache<TestTypeFullAppendix>.Properties[2].Name);
            Assert.AreEqual(nameof(TestTypeFullAppendix.OutputParameter), TypeInfoCache<TestTypeFullAppendix>.Properties[3].Name);
        }

        [TestMethod]
        public void TypeCodes()
        {
            Assert.AreEqual(TypeInfoCache<TestTypeFullAppendix>.Properties.Length, TypeInfoCache<TestTypeFullAppendix>.TypeCodes.Length);
            Assert.AreEqual(TypeCode.String, TypeInfoCache<TestTypeFullAppendix>.TypeCodes[0]);
            Assert.AreEqual(TypeCode.Int32, TypeInfoCache<TestTypeFullAppendix>.TypeCodes[3]);
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
