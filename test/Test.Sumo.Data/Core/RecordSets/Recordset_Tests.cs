using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sumo.Data
{
    [TestClass]
    public class Recordset_Tests
    {
        [TestMethod]
        public void RecordSet_Constructor_DataTable()
        {
            Recordset recordset = null;
            using (var dataset = TestDataProvider.GetDataSet())
            {
                Assert.IsTrue(dataset.Tables.Count > 0);
                using (var table = dataset.Tables[0])
                {
                    recordset = new Recordset(table);
                    Assert.AreEqual(table.Columns.Count, recordset.Fields.Length);
                    Assert.AreEqual(table.Rows.Count, recordset.Records.Length);
                    for (var i = 0; i < table.Rows.Count; ++i)
                    {
                        for (var j = 0; j < table.Columns.Count; ++j)
                        {
                            Assert.AreEqual(table.Rows[i][j], recordset[i][j]);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void RecordSet_Constructor_DataTable_GarbageCollection()
        {
            Recordset recordset = null;
            using (var dataset = TestDataProvider.GetDataSet())
            {
                Assert.IsTrue(dataset.Tables.Count > 0);
                using (var table = dataset.Tables[0])
                {
                    recordset = new Recordset(table);
                    Assert.AreEqual(table.Columns.Count, recordset.Fields.Length);
                    Assert.AreEqual(table.Rows.Count, recordset.Records.Length);
                }
            }

            System.GC.Collect();
            System.GC.WaitForFullGCComplete();

            using (var dataset = TestDataProvider.GetDataSet())
            {
                Assert.IsTrue(dataset.Tables.Count > 0);
                using (var table = dataset.Tables[0])
                {
                    Assert.AreEqual(table.Columns.Count, recordset.Fields.Length);
                    Assert.AreEqual(table.Rows.Count, recordset.Records.Length);

                    Assert.AreEqual(table.Columns.Count, recordset.Fields.Length);
                    Assert.AreEqual(table.Rows.Count, recordset.Records.Length);
                    for (var i = 0; i < table.Rows.Count; ++i)
                    {
                        for (var j = 0; j < table.Columns.Count; ++j)
                        {
                            Assert.AreEqual(table.Rows[i][j], recordset[i][j]);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void RecordSet_Constructor_RowsOnly()
        {
            Recordset recordset = null;
            using (var dataset = TestDataProvider.GetDataSet())
            {
                Assert.IsTrue(dataset.Tables.Count > 0);
                using (var table = dataset.Tables[0])
                {
                    var rows = new object[table.Rows.Count][];
                    for (var i = 0; i < table.Rows.Count; ++i)
                    {
                        rows[i] = table.Rows[i].ItemArray;
                    }
                    
                    recordset = new Recordset(table.TableName, rows);

                    Assert.AreEqual(table.Columns.Count, recordset.Fields.Length);
                    Assert.AreEqual(table.Rows.Count, recordset.Records.Length);
                    for (var i = 0; i < table.Rows.Count; ++i)
                    {
                        for (var j = 0; j < table.Columns.Count; ++j)
                        {
                            Assert.AreEqual(table.Rows[i][j], recordset[i][j]);
                        }
                    }
                }
            }
        }
    }
}


