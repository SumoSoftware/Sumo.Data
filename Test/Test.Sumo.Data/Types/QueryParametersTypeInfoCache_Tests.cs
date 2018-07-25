using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.Support;

namespace Sumo.Data.Types
{
    [TestClass]
    public class QueryParametersTypeInfoCache_Tests
    {
        [TestMethod]
        public void FullName()
        {
            Assert.AreEqual(typeof(TestTypeFullAppendix).FullName, QueryParametersTypeInfoCache<TestTypeFullAppendix>.FullName);
        }

        [TestMethod]
        public void Parameters()
        {
            Assert.AreEqual(4, QueryParametersTypeInfoCache<TestTypeFullAppendix>.Parameters.Length);
        }

        [TestMethod]
        public void TypeCodes()
        {
            Assert.AreEqual(QueryParametersTypeInfoCache<TestTypeFullAppendix>.Parameters.Length,
                QueryParametersTypeInfoCache<TestTypeFullAppendix>.TypeCodes.Length);
        }
    }
}
