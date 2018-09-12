using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Sumo.Data
{
    [TestClass]
    public class Procedure_Tests
    {
        [TestMethod]
        public void Constructor_GetParameterSize()
        {
            var parameterInfo = ProcedureParametersTypeInfoCache<TestTypeParameterSize>.OutputParameters[0];
            var parameterSize = Procedure.GetParameterSize(parameterInfo);
            Assert.AreEqual(256, parameterSize);

            parameterInfo = ProcedureParametersTypeInfoCache<TestTypeParameterSize>.OutputParameters[1];
            parameterSize = Procedure.GetParameterSize(parameterInfo);
            Assert.AreEqual(-1, parameterSize);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_GetParameterSize_PropertyNull()
        {
            PropertyInfo propertyInfo = null;
            var size= Procedure.GetParameterSize(propertyInfo);
        }

        [TestMethod]
        public void Constructor_GetInputParameter()
        {
            Assert.IsTrue(ProcedureParametersTypeInfoCache<TestTypeParameterSize>.InputParameters.Length == 1);
            var parameterInfo = ProcedureParametersTypeInfoCache<TestTypeParameterSize>.InputParameters[0];
            Assert.IsNotNull(parameterInfo);
        }

        [TestMethod]
        public void Constructor_GetOutputParameters()
        {
            Assert.IsTrue(ProcedureParametersTypeInfoCache<TestTypeParameterSize>.OutputParameters.Length == 2);
            var parameterInfo = ProcedureParametersTypeInfoCache<TestTypeParameterSize>.OutputParameters[0];
            Assert.IsNotNull(parameterInfo);
            parameterInfo = ProcedureParametersTypeInfoCache<TestTypeParameterSize>.OutputParameters[1];
            Assert.IsNotNull(parameterInfo);
        }
    }
}
