namespace Sumo.Data.Names.Sqlite
{
    public class SqliteParameterName : ParameterName
    {
        protected SqliteParameterName() : base() { }

        public SqliteParameterName(string name) : base(name) { }

        public SqliteParameterName(string name, int index) : base(name, index) { }

        public override string ToString()
        {
            return Index != -1 ? $"@{Name}_{Index}" : $"@{Name}";
        }

        public static implicit operator string(SqliteParameterName parameterName)
        {
            return parameterName.ToString();
        }
    }
}
