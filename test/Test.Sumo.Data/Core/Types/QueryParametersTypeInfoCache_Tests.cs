using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sumo.Data
{
    [TestClass]
    public class QueryParametersTypeInfoCache_Tests
    {
        [TestMethod]
        public void FullName()
        {
            Assert.AreEqual(typeof(TestTypePrefixAndCustomNameIgnoreColumn).FullName, QueryParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.FullName);
        }

        [TestMethod]
        public void Parameters()
        {
            Assert.AreEqual(4, QueryParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Parameters.Length);
        }

        [TestMethod]
        public void TypeCodes()
        {
            Assert.AreEqual(QueryParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.Parameters.Length,
                QueryParametersTypeInfoCache<TestTypePrefixAndCustomNameIgnoreColumn>.TypeCodes.Length);
        }
    }
}
