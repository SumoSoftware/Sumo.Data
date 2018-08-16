using System;

namespace Sumo.Data
{
    public class PropertyNameAttribute : DataAttribute
    {
        public PropertyNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterNameAttribute : PropertyNameAttribute
    {
        public ParameterNameAttribute(string name) : base(name) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : ParameterNameAttribute
    {
        public ColumnNameAttribute(string name) : base(name) { }
    }
}
