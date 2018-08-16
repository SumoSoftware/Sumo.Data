namespace Sumo.Data.Schema.SqlServer
{
    internal class SqlServerScriptBuilder : IScriptBuilder
    {
        public string BuildAlterTableScript(TableDefinition table, ColumnDefinition[] columnsToAdd, ColumnDefinition[] columnsToRemove)
        {
            throw new System.NotImplementedException();
        }

        public string BuildCreateScript(CatalogDefinition catalog)
        {
            throw new System.NotImplementedException();
        }

        public string BuildCreateScript(SchemaDefinition schema)
        {
            throw new System.NotImplementedException();
        }

        public string BuildCreateScript(TableDefinition table)
        {
            throw new System.NotImplementedException();
        }

        public TableDefinition BuildTable<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}