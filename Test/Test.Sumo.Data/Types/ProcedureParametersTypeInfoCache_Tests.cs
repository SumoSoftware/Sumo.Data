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
            Assert.AreEqual(typeof(TestTypeFullAppendix).FullName, ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.FullName);
        }

        [TestMethod]
        public void InputParameters()
        {
            Assert.AreEqual(3, ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.InputParameters.Length);
            Assert.AreEqual(nameof(TestTypeFullAppendix.ReadWriteProperty), ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.InputParameters[0].Name);
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
            Assert.AreEqual(1, ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.OutputParameters.Length);
            Assert.AreEqual(nameof(TestTypeFullAppendix.OutputParameter), ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.OutputParameters[0].Name);
        }

        [TestMethod]
        public void InputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.InputParameters.Length, 
                ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.InputTypeCodes.Length);
        }

        [TestMethod]
        public void InputOutputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.InputOutputParameters.Length, 
                ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.InputOutputTypeCodes.Length);
        }

        [TestMethod]
        public void OutputTypeCodes()
        {
            Assert.AreEqual(ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.OutputParameters.Length,
                ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.OutputTypeCodes.Length);
        }

        [TestMethod]
        public void ProcedureName()
        {
            Assert.AreEqual($"[prefix].[{nameof(TestTypeFullAppendix)}].[suffix]", ProcedureParametersTypeInfoCache<TestTypeFullAppendix>.ProcedureName);
            Assert.AreEqual($"[prefix].[{nameof(TestTypePrefix)}]", ProcedureParametersTypeInfoCache<TestTypePrefix>.ProcedureName);
            Assert.AreEqual($"[{nameof(TestTypeSuffix)}].[suffix]", ProcedureParametersTypeInfoCache<TestTypeSuffix>.ProcedureName);
        }
    }
}
