using System;

namespace Sumo.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PropertyNameAttribute : DataAttribute
    {
        public PropertyNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : PropertyNameAttribute
    {
        public ColumnNameAttribute(string name) : base(name) { }
    }
}
