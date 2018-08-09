namespace Sumo.Data.Schema.SqlServer
{
    internal class SqlServerScriptBuilder : IScriptBuilder
    {
        public string BuildAlterTableScript(Table table, Column[] columnsToAdd, Column[] columnsToRemove)
        {
            throw new System.NotImplementedException();
        }

        public string BuildCreateScript(Catalog catalog)
        {
            throw new System.NotImplementedException();
        }

        public string BuildCreateScript(Schema schema)
        {
            throw new System.NotImplementedException();
        }

        public string BuildCreateScript(Table table)
        {
            throw new System.NotImplementedException();
        }

        public Table BuildTable<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}