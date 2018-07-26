using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sumo.Data.Orm.Extensions;
using Sumo.Data.Support;
using System.Threading.Tasks;

namespace Sumo.Data.Types
{
    [TestClass]
    public class TypeExtensions_Tests
    {
        [TestMethod]
        public void DataRow_ToObject()
        {
            var row = TestDataProvider.GetRow();
            var testType = row.ToObject<TestType>();

            Assert.AreEqual("row one", testType.ReadWriteString);
            Assert.AreEqual(1, testType.ReadWriteInteger);

            Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            Assert.AreEqual("ReadString", testType.ReadString);
            Assert.AreEqual("IgnoreString", testType.IgnoreString);
        }

        [TestMethod]
        public void DataRows_ToArray()
        {
            var rows = TestDataProvider.GetRows();
            var testTypes = rows.ToArray<TestType>();
            var testType = testTypes[0];

            Assert.AreEqual("row one", testType.ReadWriteString);
            Assert.AreEqual(1, testType.ReadWriteInteger);

            Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            Assert.AreEqual("ReadString", testType.ReadString);
            Assert.AreEqual("IgnoreString", testType.IgnoreString);

            testType = testTypes[1];

            Assert.AreEqual("row two", testType.ReadWriteString);
            Assert.AreEqual(2, testType.ReadWriteInteger);

            Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            Assert.AreEqual("ReadString", testType.ReadString);
            Assert.AreEqual("IgnoreString", testType.IgnoreString);
        }

        [TestMethod]
        public async Task DataRows_ToArrayAsync()
        {
            var rows = TestDataProvider.GetRows();
            var testTypes = await rows.ToArrayAsync<TestType>();
            var testType = testTypes[0];

            Assert.AreEqual("row one", testType.ReadWriteString);
            Assert.AreEqual(1, testType.ReadWriteInteger);

            Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            Assert.AreEqual("ReadString", testType.ReadString);
            Assert.AreEqual("IgnoreString", testType.IgnoreString);

            testType = testTypes[1];

            Assert.AreEqual("row two", testType.ReadWriteString);
            Assert.AreEqual(2, testType.ReadWriteInteger);

            Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            Assert.AreEqual("ReadString", testType.ReadString);
            Assert.AreEqual("IgnoreString", testType.IgnoreString);
        }

        [TestMethod]
        public void DataRows_ToArrayParallel()
        {
            var rows = TestDataProvider.GetRows();
            var testTypes = rows.ToArrayParallel<TestType>();
            var testType = testTypes[0];

            Assert.AreEqual("row one", testType.ReadWriteString);
            Assert.AreEqual(1, testType.ReadWriteInteger);

            Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            Assert.AreEqual("ReadString", testType.ReadString);
            Assert.AreEqual("IgnoreString", testType.IgnoreString);

            testType = testTypes[1];

            Assert.AreEqual("row two", testType.ReadWriteString);
            Assert.AreEqual(2, testType.ReadWriteInteger);

            Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            Assert.AreEqual("ReadString", testType.ReadString);
            Assert.AreEqual("IgnoreString", testType.IgnoreString);
        }

        [TestMethod]
        public void DataRows_ToJson()
        {
            using (var dataset = TestDataProvider.GetDataSet())
            {
                var tableJson = JsonConvert.SerializeObject(dataset.Tables[0], Formatting.Indented);
                var rows = dataset.Tables[0].Rows;
                var objects = rows.ToArray<TestType>();
                var objectJson = JsonConvert.SerializeObject(objects, Formatting.Indented);
            }

            //Assert.AreEqual("row one", testType.ReadWriteString);
            //Assert.AreEqual(1, testType.ReadWriteInteger);

            //Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            //Assert.AreEqual("ReadString", testType.ReadString);
            //Assert.AreEqual("IgnoreString", testType.IgnoreString);

            //testType = testTypes[1];

            //Assert.AreEqual("row two", testType.ReadWriteString);
            //Assert.AreEqual(2, testType.ReadWriteInteger);

            //Assert.AreEqual("ReadPrivateWriteString", testType.ReadPrivateWriteString);
            //Assert.AreEqual("ReadString", testType.ReadString);
            //Assert.AreEqual("IgnoreString", testType.IgnoreString);
        }
    }
}
