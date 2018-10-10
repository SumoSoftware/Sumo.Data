using Sumo.Data.Schema;
using Sumo.Data.Schema.Sqlite;
using Sumo.Data.Sqlite;
using Sumo.Retry;

namespace Sumo.Data.Orm.Sqlite
{
    //todo: add IConnectionStringFactory support
    public class SqliteOrmDataComponentFactory : SqliteDataComponentFactory, IOrmDataComponentFactory
    {

        public SqliteOrmDataComponentFactory(string connectionString) : base(connectionString)
        {
            SchemaParameterNames = new SqliteSchemaParameterNames();
            ScriptBuilder = new SqliteScriptBuilder();
            SqlStatementBuilder = new SqliteStatementBuilder(this);
        }

        public SqliteOrmDataComponentFactory(SqliteTransientRetryPolicy retryPolicy) : base(retryPolicy)
        {
            SchemaParameterNames = new SqliteSchemaParameterNames();
            ScriptBuilder = new SqliteScriptBuilder();
            SqlStatementBuilder = new SqliteStatementBuilder(this);
        }

        public SqliteOrmDataComponentFactory(SqliteTransientRetryPolicy retryPolicy, string connectionString) : base(retryPolicy, connectionString)
        {
            SchemaParameterNames = new SqliteSchemaParameterNames();
            ScriptBuilder = new SqliteScriptBuilder();
            SqlStatementBuilder = new SqliteStatementBuilder(this);
        }

        public ISchemaParameterNames SchemaParameterNames { get; }
        public IScriptBuilder ScriptBuilder { get; }
        public ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
