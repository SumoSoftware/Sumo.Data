using Sumo.Data.Schema;
using Sumo.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sumo.Data.Schema.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.SqlServer;

namespace Test.Sumo.Sync.SyncTests
{
    [TestClass]
    public class HeaderSyncTests
    {
        class Manifest
        {
            public int table_index { get; set; }
            public string table_name { get; set; }
            public string primary_key { get; set; }
            public int record_count { get; set; }
            public bool full_sync { get; set; }
            public float sync_api { get; set; }
        }

        [EntityName("usp_MOB_GetSyncData")]
        public class SyncProcParams
        {
            public DateTime LastSync { get; set; }
            public float PreviousSyncApiId { get; set; }
            public String ClientId { get; set; }
            public int UserId { get; set; }
        }

        [TestMethod]
        public void FullTest()
        {
            IDataComponentFactory connectionFactory;
            var entFactory = new EntityFactory();

            //Add your SQLServer Connection String as environment variable "TESTCONNSTRING"
            connectionFactory = new SqlServerDataComponentFactory(Environment.GetEnvironmentVariable("TESTCONNSTRING"));

            var prm = new SyncProcParams()
            {
                ClientId = "CAFEFRESH",
                LastSync = DateTime.Now,
                UserId = 44720,
                PreviousSyncApiId = 0.0f
            };

            using (var outputStream = new MemoryStream())
            {
                using (var proc = new ReadProcedure(connectionFactory))
                {
                    var readResult = proc.Read(prm);
                    var ds = readResult.DataSet;

                    var rows = ds.Tables[ds.Tables.Count - 1];
                    var manifest = ds.Tables[ds.Tables.Count - 1].Rows.ToArray<Manifest>();

                    var binaryWriter = new BinaryWriter(outputStream);
                    binaryWriter.Write(manifest.Length);

                    foreach (var manifestTbl in manifest)
                    {
                        var dataTable = ds.Tables[manifestTbl.table_index];
                        var outputTable = entFactory.ToTable(dataTable, manifestTbl.table_name);
                        var pkeyColumn = outputTable.Columns.Where(col => col.Name == manifestTbl.primary_key).FirstOrDefault();
                        if (pkeyColumn != null) pkeyColumn.IsPrimaryKey = true;
                        outputStream.Write(outputTable);
                        binaryWriter.Write(dataTable.Rows.Count);
                        binaryWriter.WriteToStream(outputTable, dataTable.Rows);
                    }
                }

                outputStream.Seek(0, SeekOrigin.Begin);

                var binaryReader = new BinaryReader(outputStream);
                var tableCount = binaryReader.ReadInt32();
                for (var idx = 0; idx < tableCount; ++idx)
                {
                    var tbl = outputStream.ReadFromStream<Table>();
                    var rowCount = binaryReader.ReadInt32();
                    for(var rowIdx = 0; rowIdx < rowCount; ++rowIdx)
                    {
                        var values = binaryReader.ReadFromStream(tbl);
                        /* Code to write the object array  to SQLite in a transaction */
                    }
                    
                }
            }
        }
    }
}
