using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Support;

namespace Sumo.Data.Types
{
    [TestClass]
    public class ProcedureParametersTypeInfoCache_Tests
    {
        [TestMethod]
        public void FullName()
        {
            Assert.AreEqual(typeof(TestTypePrefixAndCustomName).FullName, ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.FullName);
        }

        [TestMethod]
        public void InputParameters()
        {
            Assert.AreEqual(3, ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.InputParameters.Length);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomName.ReadWriteProperty), ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.InputParameters[0].Name);
        }

        [TestMethod]
        public void InputOutputParameters()
        {
            Assert.AreEqual(1, ProcedureParametersTypeInfoCache<TestTypeInputOutputParams>.InputOutputParameters.Length);
            Assert.AreEqual(nameof(TestTypeInputOutputParams.InputOutputParameter), ProcedureParametersTypeInfoCache<TestTypeInputOutputParams>.InputOutputParameters[0].Name);
        }

        [TestMethod]
        public void OutputParameters()
        {
            Assert.AreEqual(1, ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.OutputParameters.Length);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomName.OutputParameter), ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.OutputParameters[0].Name);
        }

        [TestMethod]
        public void InputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.InputParameters.Length, 
                ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.InputTypeCodes.Length);
        }

        [TestMethod]
        public void InputOutputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.InputOutputParameters.Length, 
                ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.InputOutputTypeCodes.Length);
        }

        [TestMethod]
        public void OutputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.OutputParameters.Length,
                ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.OutputTypeCodes.Length);
        }

        [TestMethod]
        public void ProcedureName()
        {
            Assert.AreEqual($"[prefix].[{nameof(TestTypePrefix)}]", ProcedureParametersTypeInfoCache<TestTypePrefix>.ProcedureName);
            Assert.AreEqual($"[prefix].[test_name]", ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomName>.ProcedureName);
        }
    }
}
