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
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_GetParameterSize_PropertyMissingParameterSizeAttribute()
        {
            var parameterInfo = ProcedureParametersTypeInfoCache<TestTypeParameterSize>.InputParameters[0];
            var parameterSize = Procedure.GetParameterSize(parameterInfo);
        }
    }
}
