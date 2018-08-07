using System;

namespace Sumo.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityNameAttribute : Attribute
    {
        public EntityNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EntityPrefixAttribute : Attribute
    {
        public EntityPrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; }
    }
}
