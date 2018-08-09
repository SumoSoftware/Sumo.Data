using Sumo.Data.Schema;
using Sumo.Data;
using System.Linq;
using System;
using System.IO;
using Sumo.Data.Schema.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data.SqlServer;
using Sumo.Data.Sqlite;
using Test.Sumo.Sync.Utils;
using Sumo.Data.Orm.Sqlite;

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
        

        public const string OUTPUT_FILENAME = @"L:\SQLITEDB1.sqlite";

        [TestMethod]
        public void GetRecords()
        {
            var sqlLiteConnectionString = $"Data Source={OUTPUT_FILENAME}";
            var dataContext = new DataContext(new SqliteOrmDataComponentFactory(sqlLiteConnectionString));
            var stores = dataContext.GetStores();

        }

        [TestMethod]
        public void FullTest()
        {
            IDataComponentFactory connectionFactory;
            IDataComponentFactory sqliteConnectionFactory;
            var entFactory = new EntityFactory();

            var outputFileName = OUTPUT_FILENAME;
            var sqlLiteConnectionString = $"Data Source={outputFileName}";

            if (System.IO.File.Exists(outputFileName))
            {
                System.IO.File.Delete(outputFileName);
            }

            //Add your SQLServer Connection String as environment variable "TESTCONNSTRING"
            connectionFactory = new SqlServerDataComponentFactory(Environment.GetEnvironmentVariable("TESTCONNSTRING"));
            sqliteConnectionFactory = new SqliteDataComponentFactory(sqlLiteConnectionString);

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
                    var count = manifest.Length;
                    binaryWriter.Write(count);

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

                var recordCount = 0;

                var sqliteConnection = sqliteConnectionFactory.Open();

                var binaryReader = new BinaryReader(outputStream);
                var tableCount = binaryReader.ReadInt32();
                for (var idx = 0; idx < tableCount; ++idx)
                {
                    var batchWriter = new BatchWriter();
                    
                    var tbl = outputStream.ReadFromStream<Table>();

                    batchWriter.Init(tbl, sqliteConnection);
                    batchWriter.Begin();
                    var rowCount = binaryReader.ReadInt32();
                    for(var rowIdx = 0; rowIdx < rowCount; ++rowIdx)
                    {
                        var values = binaryReader.ReadRowFromStream(tbl);
                        batchWriter.Execute(values);
                        recordCount++;
                        /* Code to write the object array  to SQLite in a transaction */
                    }
                    batchWriter.End();                    
                }
            }
        }
    }
}
