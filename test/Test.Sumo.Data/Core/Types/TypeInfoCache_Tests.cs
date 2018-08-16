using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sumo.Data
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

        public class ParamNameTestClass
        {
            [PropertyName("pname1")]
            public int NamedParam1 { get; set; }

            [ColumnName("pname2")]
            public int NamedParam2 { get; set; }

            [ParameterName("pname3")]
            public int NamedParam3 { get; set; }
        }

        [TestMethod]
        public void ParamNameTest()
        {
            Assert.AreEqual("pname1", TypeInfoCache<ParamNameTestClass>.PropertyNames[0]);
            Assert.AreEqual("pname2", TypeInfoCache<ParamNameTestClass>.PropertyNames[1]);
            Assert.AreEqual("pname3", TypeInfoCache<ParamNameTestClass>.PropertyNames[2]);
        }

    }
}
