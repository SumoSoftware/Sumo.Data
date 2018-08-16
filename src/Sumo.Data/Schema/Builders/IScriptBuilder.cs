namespace Sumo.Data.Schema
{
    public interface IScriptBuilder
    {
        string BuildCreateScript(CatalogDefinition catalog);
        string BuildCreateScript(SchemaDefinition schema);
        string BuildCreateScript(TableDefinition table);

        string BuildAlterTableScript(TableDefinition table, ColumnDefinition[] columnsToAdd, ColumnDefinition[] columnsToRemove);

        TableDefinition BuildTable<T>() where T : class;
    }
}
