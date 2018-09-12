using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sumo.Data
{
    [TestClass]
    public class ProcedureParametersTypeInfoCache_Tests
    {
        [TestMethod]
        public void FullName()
        {
            Assert.AreEqual(typeof(TestTypePrefixAndCustomNameIgnoreColumn).FullName, ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.FullName);
        }

        [TestMethod]
        public void InputParameters()
        {
            Assert.AreEqual(3, ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreParameter>.InputParameters.Length);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomNameIgnoreParameter.ReadWriteProperty), ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreParameter>.InputParameters[0].Name);
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
            Assert.AreEqual(1, ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.OutputParameters.Length);
            Assert.AreEqual(nameof(TestTypePrefixAndCustomNameIgnoreColumn.OutputParameter), ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.OutputParameters[0].Name);
        }

        [TestMethod]
        public void InputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.InputParameters.Length, 
                ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.InputTypeCodes.Length);
        }

        [TestMethod]
        public void InputOutputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.InputOutputParameters.Length, 
                ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.InputOutputTypeCodes.Length);
        }

        [TestMethod]
        public void OutputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.OutputParameters.Length,
                ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.OutputTypeCodes.Length);
        }

        [TestMethod]
        public void ProcedureName()
        {
            Assert.AreEqual($"[prefix].[{nameof(TestTypePrefix)}]", ProcedureParametersTypeInfoCache<TestTypePrefix>.ProcedureName);
            Assert.AreEqual($"[prefix].[test_name]", ProcedureParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.ProcedureName);
        }
    }
}
