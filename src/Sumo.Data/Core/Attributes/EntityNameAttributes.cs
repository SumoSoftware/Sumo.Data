using System;

namespace Sumo.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityNameAttribute : DataAttribute
    {
        public EntityNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EntityPrefixAttribute : DataAttribute
    {
        public EntityPrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; }
    }
}
