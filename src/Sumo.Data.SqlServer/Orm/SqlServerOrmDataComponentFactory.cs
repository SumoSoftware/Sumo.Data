using Sumo.Data.Schema;
using Sumo.Data.Schema.SqlServer;
using Sumo.Data.SqlServer;
using Sumo.Retry;

namespace Sumo.Data.Orm.SqlServer
{
    //todo: add IConnectionStringFactory support
    public class SqlServerOrmDataComponentFactory : SqlServerDataComponentFactory, IOrmDataComponentFactory
    {

        public SqlServerOrmDataComponentFactory(string connectionString) : base(connectionString)
        {
            SchemaParameterNames = new SqlServerSchemaParameterNames();
            ScriptBuilder = new SqlServerScriptBuilder();
            SqlStatementBuilder = new SqlServerSqlStatementBuilder(this);
        }

        public SqlServerOrmDataComponentFactory(SqlServerTransientRetryPolicy retryPolicy) : base(retryPolicy)
        {
            SchemaParameterNames = new SqlServerSchemaParameterNames();
            ScriptBuilder = new SqlServerScriptBuilder();
            SqlStatementBuilder = new SqlServerSqlStatementBuilder(this);
        }

        public SqlServerOrmDataComponentFactory(SqlServerTransientRetryPolicy retryPolicy, string connectionString) : base(retryPolicy, connectionString)
        {
            SchemaParameterNames = new SqlServerSchemaParameterNames();
            ScriptBuilder = new SqlServerScriptBuilder();
            SqlStatementBuilder = new SqlServerSqlStatementBuilder(this);
        }

        public ISchemaParameterNames SchemaParameterNames { get; }
        public IScriptBuilder ScriptBuilder { get; }
        public ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
