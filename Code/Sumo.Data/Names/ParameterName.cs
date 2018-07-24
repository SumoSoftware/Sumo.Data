namespace Sumo.Data.Names
{
    public class ParameterName : ItemName, IParameterName
    {
        protected ParameterName() : base() { }

        public ParameterName(string name) : base(name) { }

        public bool Equals(IParameterName other)
        {
            return base.Equals(other);
        }

        public override string ToString()
        {
            return $"@{Name}";
        }

        public static implicit operator string(ParameterName parameterName)
        {
            return parameterName.ToString();
        }
    }
}
