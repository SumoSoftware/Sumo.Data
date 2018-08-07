using Sumo.Data.Factories;
using Sumo.Data.Factories.SqlServer;
using Sumo.Data.Procedures;
using System;
using System.Data;

namespace Sumo.Data.Generator
{
    class Program
    {

        class usp_MOB_GetSyncData
        {
            public DateTime LastSync { get; set; }
            public float PreviousSyncApiId { get; set; }
            public String ClientId { get; set; }
            public int UserId { get; set; }
        }

        static void Main(string[] args)
        {
            IConnectionFactory connectionFactory;        
            connectionFactory = new SqlServerConnectionFactory(/*ADD YOUR CONNECTION STRING*/);

            var prm = new usp_MOB_GetSyncData()
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
                foreach (DataTable tbl in ds.Tables)
                {
                    Console.WriteLine(tbl.TableName);
                }
            }            

            Console.ReadKey();
        }
    }
}
