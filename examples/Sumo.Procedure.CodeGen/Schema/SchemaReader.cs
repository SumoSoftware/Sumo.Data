using Sumo.Data;
using Sumo.Data.SqlServer;
using Sumo.Procedure.CodeGen.Properties;
using System;
using System.Linq;

namespace Sumo.Procedure.CodeGen
{
    public static class SchemaReader
    {
        public static Schema ReadSchema(string connectionString)
        {
            var result = new Schema();

            var retryPolicy = new SqlServerTransientRetryPolicy(30, TimeSpan.FromSeconds(60));
            IConnectionFactory connectionFactory = new SqlServerConnectionFactoryWithRetry(retryPolicy);

            using (var connection = connectionFactory.Open(connectionString))
            using (var reader = new SqlReader(connection, new SqlServerParameterFactory(), new SqlServerDataAdapterFactory()))
            {
                using (var schemas = reader.Read(Resources.GetSchemasSql))
                {
                    if (schemas.Tables.Count != 1)
                    {
                        throw new ApplicationException("no schemas found");
                    }

                    result.Schemas = schemas.Tables[0].Rows.ToArray<IItemName, ItemName>();
                }
                using (var tables = reader.Read(Resources.GetTablesSql))
                {
                    if (tables.Tables.Count != 1)
                    {
                        throw new ApplicationException("no tables found");
                    }

                    result.Tables = tables.Tables[0].Rows.ToArray<IEntityName, EntityName>();
                }
                using (var procedures = reader.Read(Resources.GetProceduresSql))
                {
                    if (procedures.Tables.Count != 1)
                    {
                        throw new ApplicationException("no procedures found");
                    }

                    result.Procedures = procedures.Tables[0].Rows.ToArray<Procedure>().ToDictionary(p => p.ToString());
                }
                using (var procedureParams = reader.Read(Resources.GetProcedureParametersSql))
                {
                    if (procedureParams.Tables.Count != 1)
                    {
                        throw new ApplicationException("no parameters found");
                    }

                    var parameters = procedureParams.Tables[0].Rows.ToArray<ProcedureParameter>();
                    foreach (var procedure in result.Procedures)
                    {
                        procedure.Value.ProcedureParameters = (from p in parameters where p.Schema == procedure.Value.Schema && p.Procedure == procedure.Value.Name orderby p.Order select p).ToArray();
                    }
                }
            }

            return result;
        }
    }
}
