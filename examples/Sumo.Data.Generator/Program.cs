﻿using Sumo.Data.Schema;
using Sumo.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sumo.Data.Generator
{
    class Program
    {
        [EntityName("usp_MOB_GetSyncData")]
        class UspMOBGetSyncData
        {
            public DateTime LastSync { get; set; }
            public float PreviousSyncApiId { get; set; }
            public string ClientId { get; set; }
            public int UserId { get; set; }
        }

        static void Main(string[] args)
        {
            IDataComponentFactory connectionFactory;
            var entFactory = new EntityDefinitionFactory();
            var codeGen = new CSharpCodeGen();

            //Add your SQLServer Connection String as environment variable "TESTCONNSTRING"
            connectionFactory = new SqlServerDataComponentFactory(Environment.GetEnvironmentVariable("TESTCONNSTRING"));

            var prm = new UspMOBGetSyncData()
            {
                ClientId = "CAFEFRESH",
                LastSync = DateTime.Now,
                UserId = 44720,
                PreviousSyncApiId = 0.0f
            };

            using (var proc = new ReadProcedure(connectionFactory))
            {
                var readResult = proc.Read(prm);
                DataSet ds = readResult.DataSet;

                var tables = new List<TableDefinition>();

                var rows = ds.Tables[ds.Tables.Count - 1];
                var manifest = ds.Tables[ds.Tables.Count - 1].Rows.ToArray<Models.Manifest>();
                foreach(var manifestTbl in manifest)
                {
                    var dataTable = ds.Tables[manifestTbl.table_index];
                    var outputTable = entFactory.ToTable(dataTable, manifestTbl.table_name);
                    var pkeyColumn = outputTable.Columns.Where(col => col.Name == manifestTbl.primary_key).FirstOrDefault();
                    if(pkeyColumn != null) pkeyColumn.IsPrimaryKey = true;
                    tables.Add(outputTable);
                }

                 System.IO.File.WriteAllText("L:\\foo.cs",codeGen.ToFile(tables, "GSP.X.Repository.Local", true));

            }

            Console.ReadKey();
        }
    }
}