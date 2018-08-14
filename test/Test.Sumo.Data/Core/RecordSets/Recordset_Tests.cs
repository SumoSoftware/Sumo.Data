using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sumo.Data
{
    [TestClass]
    public class Recordset_Tests
    {
        [TestMethod]
        public void RecordSet_Constructor_DataTable()
        {
            var dataset = TestDataProvider.GetDataSet();

            Assert.IsTrue(dataset.Tables.Count > 0);
            var table = dataset.Tables[0];

            var recordset = new Recordset(table);
            Assert.AreEqual(table.Columns.Count, recordset.Fields.Length);
            Assert.AreEqual(table.Rows.Count, recordset.Records.Length);
            for (var i = 0; i < table.Rows.Count; ++i)
            {
                for (var j = 0; j < table.Columns.Count; ++j)
                {
                    Assert.AreEqual(table.Rows[i][j], recordset.Records[i][j]);
                }
            }

        }
    }

}


