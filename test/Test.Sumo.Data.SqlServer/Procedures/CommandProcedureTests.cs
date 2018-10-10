using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sumo.Data;
using Sumo.Data.SqlServer;
using System;
using System.Threading.Tasks;

/*
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [Test].[ParamContextTest]
	@InputDate datetime,
	@OutputDate datetime = null output
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	set @OutputDate = @InputDate;

    -- Insert statements for procedure here
    return 99;
END
*/


namespace Test.Sumo.Data.SqlServer.Procedures
{
    [EntityName("ParamContextTest"), EntityPrefix("Test")]
    public sealed class InputOutputProcedureContext
    {
        [InputParameter("InputDate")]
        public DateTime? InDate { get; set; } = DateTime.Now;

        [InputOutputParameter("OutputDate")]
        public DateTime? OutDate { get; set; } = null;
    }

    [TestClass]
    public class CommandProcedureTests
    {
        [TestMethod]
        public async Task CommandProcedure_ExecuteAsync_ParameterContextWithNullableInputAndOutputParams()
        {
            var procedureContext = new InputOutputProcedureContext();

            var dataComponentFactory = new SqlServerDataComponentFactory(new SqlServerTransientRetryPolicy(60, TimeSpan.FromSeconds(30)), AppState.ConnectionString);
            using (var connection = dataComponentFactory.Open())
            using (var procedure = new CommandProcedure(connection, dataComponentFactory))
            {
                var result = await procedure.ExecuteAsync(procedureContext);
                Assert.AreEqual(99, result);
            }

            Assert.IsTrue(procedureContext.InDate.HasValue);
            Assert.IsTrue(procedureContext.OutDate.HasValue);
            Assert.AreEqual(procedureContext.InDate.Value.ToString(), procedureContext.OutDate.Value.ToString());
        }
    }
}