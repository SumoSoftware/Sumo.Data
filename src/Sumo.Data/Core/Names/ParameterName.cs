namespace Sumo.Data
{
    public class ParameterName : ItemName, IParameterName
    {
        protected ParameterName() : base() { }

        public ParameterName(string name) : this(name, -1) { }

        public ParameterName(string name, int index) : base(name)
        {
            Index = index;
        }

        public int Index { get; } = -1;

        public bool Equals(IParameterName other)
        {
            return base.Equals(other);
        }

        public override string ToString()
        {
            return Index != -1 ? $"@{Name}_{Index}" : $"@{Name}";
        }

        public static implicit operator string(ParameterName parameterName)
        {
            return parameterName.ToString();
        }
    }
}
