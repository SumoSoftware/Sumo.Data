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

        [TestMethod]
        public void RecordSet_Append_Array()
        {
            var size = 2;
            var rows = new object[size][];
            var c = 0;
            for (var i = 0; i < size; ++i)
            {
                rows[i] = new object[size];
                for (var j = 0; j < size; ++j)
                {
                    rows[i][j] = ++c;
                }
            }

            var recordset = new Recordset("test", rows);
            Assert.AreEqual(size, recordset.Count);
            c = 0;
            for (var i = 0; i < size; ++i)
                for (var j = 0; j < size; ++j)
                    Assert.AreEqual(++c, recordset[i][j]);

            recordset.Append(rows);
            Assert.AreEqual(size * 2, recordset.Count);
            for (var o = 0; o < 2; ++o)
            {
                c = 0;
                for (var i = 0; i < size; ++i)
                    for (var j = 0; j < size; ++j)
                        Assert.AreEqual(++c, recordset[i][j]);
            }
        }
    }
}


