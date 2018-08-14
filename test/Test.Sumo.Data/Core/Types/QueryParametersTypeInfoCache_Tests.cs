using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sumo.Data
{
    [TestClass]
    public class QueryParametersTypeInfoCache_Tests
    {
        [TestMethod]
        public void FullName()
        {
            Assert.AreEqual(typeof(TestTypePrefixAndCustomName).FullName, QueryParametersTypeInfoCache<TestTypePrefixAndCustomName>.FullName);
        }

        [TestMethod]
        public void Parameters()
        {
            Assert.AreEqual(4, QueryParametersTypeInfoCache<TestTypePrefixAndCustomName>.Parameters.Length);
        }

        [TestMethod]
        public void TypeCodes()
        {
            Assert.AreEqual(QueryParametersTypeInfoCache<TestTypePrefixAndCustomName>.Parameters.Length,
                QueryParametersTypeInfoCache<TestTypePrefixAndCustomName>.TypeCodes.Length);
        }
    }
}
