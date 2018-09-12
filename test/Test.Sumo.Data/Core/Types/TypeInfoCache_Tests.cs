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
            Assert.AreEqual(typeof(TestTypePrefixAndCustomNameIgnoreColumn).FullName, TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.FullName);
        }

        [TestMethod]
        public void TestColumnAndParamAttributes()
        {
            Assert.AreEqual(1, TypeInfoCache<ColumnAndParamAttributeTest>.Properties.Length);
        }

        [TestMethod]
        public void SerializableParameters()
        {
            Assert.AreEqual(4, TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Properties.Length);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomNameIgnoreColumn.ReadWriteProperty), TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Properties[0].Name);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomNameIgnoreColumn.ReadPrivateWriteProperty), TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Properties[1].Name);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomNameIgnoreColumn.ReadProperty), TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Properties[2].Name);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomNameIgnoreColumn.OutputParameter), TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Properties[3].Name);
        }

        [TestMethod]
        public void TypeCodes()
        {
            Assert.AreEqual(TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Properties.Length, TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.TypeCodes.Length);
            Assert.AreEqual(TypeCode.String, TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.TypeCodes[0]);
            Assert.AreEqual(TypeCode.Int32, TypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.TypeCodes[3]);
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
            public int NamedParam1 { get; set; }

            [InputParameter("pname2")]
            public int NamedParam2 { get; set; }

            [InputParameter("pname3")]
            public int NamedParam3 { get; set; }
        }

        [TestMethod]
        public void ParamNameTest()
        {
            Assert.AreEqual("NamedParam1", ProcedureParametersTypeInfoCache<ParamNameTestClass>.InputParameterNames[0]);
            Assert.AreEqual("pname2", ProcedureParametersTypeInfoCache<ParamNameTestClass>.InputParameterNames[1]);
            Assert.AreEqual("pname3", ProcedureParametersTypeInfoCache<ParamNameTestClass>.InputParameterNames[2]);
        }

        public class ColumnNameClass
        {
            public int Id { get; set; }
            [ColumnName("FullName")]
            public string Name { get; set; }
        }

        [TestMethod]
        public void ColumnNameTest()
        {
            Assert.AreEqual("Id", TypeInfoCache<ColumnNameClass>.ReadWritePropertyNames[0]);
            Assert.AreEqual("FullName", TypeInfoCache<ColumnNameClass>.ReadWritePropertyNames[1]);
        }
    }
}
