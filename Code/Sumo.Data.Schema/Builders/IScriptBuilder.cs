namespace Sumo.Data.Schema
{
    public interface IScriptBuilder
    {
        string BuildDbCreateScript(Catalog catalog);
        string BuildDbCreateScript(Schema schema);
        string BuildDbCreateScript(Table table);

        Table BuildTable<T>() where T : class;
    }
}
