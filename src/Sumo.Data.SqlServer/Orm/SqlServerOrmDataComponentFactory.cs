using Sumo.Data.Schema;
using Sumo.Data.Schema.SqlServer;
using Sumo.Data.SqlServer;
using Sumo.Retry;

namespace Sumo.Data.Orm.SqlServer
{
    public class SqlServerOrmDataComponentFactory : SqlServerDataComponentFactory, IOrmDataComponentFactory
    {

        public SqlServerOrmDataComponentFactory(string connectionString) : base(connectionString)
        {
            SchemaParameterNames = new SqlServerSchemaParameterNames();
            ScriptBuilder = new SqlServerScriptBuilder();
            SqlStatementBuilder = new SqlServerSqlStatementBuilder(this);
        }

        public SqlServerOrmDataComponentFactory(RetryOptions retryOptions) : base(retryOptions)
        {
            SchemaParameterNames = new SqlServerSchemaParameterNames();
            ScriptBuilder = new SqlServerScriptBuilder();
            SqlStatementBuilder = new SqlServerSqlStatementBuilder(this);
        }

        public SqlServerOrmDataComponentFactory(RetryOptions retryOptions, string connectionString) : base(retryOptions, connectionString)
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
