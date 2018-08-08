namespace Sumo.Data.Schema
{
    public interface IScriptBuilder
    {
        string BuildCreateScript(Catalog catalog);
        string BuildCreateScript(Schema schema);
        string BuildCreateScript(Table table);

        string BuildAlterTableScript(Table table, Column[] columnsToAdd, Column[] columnsToRemove);

        Table BuildTable<T>() where T : class;
    }
}
