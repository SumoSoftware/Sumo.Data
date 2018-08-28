using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data;
using Sumo.Data.SqlServer;
using Sumo.Retry;
using System;
using System.Threading.Tasks;

namespace Test.Sumo.Data.SqlServer.Procedures
{
    [EntityName("ParamContextTest"), EntityPrefix("Test")]
    public class ProcedureContext
    {
        [InputParameter("PayoutDateUtc")]
        public DateTime? Date { get; set; }
    }

    [TestClass]
    public class CommandProcedureTests
    {
        [TestMethod]
        public async Task TestNullableParams()
        {
            var procedureContext = new ProcedureContext();

            var dataComponentFactory = new SqlServerDataComponentFactory(new RetryOptions(60, TimeSpan.FromSeconds(30)), AppState.ConnectionString);
            using (var connection = dataComponentFactory.Open())
            using (var procedure = new CommandProcedure(connection, dataComponentFactory))
            {
                var result = await procedure.ExecuteAsync(procedureContext);
            }
        }
    }
}