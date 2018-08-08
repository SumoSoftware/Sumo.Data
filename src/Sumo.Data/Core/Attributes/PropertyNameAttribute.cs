using System;

namespace Sumo.Data.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyNameAttribute : DataAttribute
    {
        public PropertyNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
