namespace Sumo.Data.Names.Sqlite
{
    public class SqliteParameterName : ParameterName
    {
        protected SqliteParameterName() : base() { }

        public SqliteParameterName(string name) : base(name) { }

        public override string ToString()
        {
            return $"${Name}";
        }

        public static implicit operator string(SqliteParameterName parameterName)
        {
            return parameterName.ToString();
        }
    }
}
