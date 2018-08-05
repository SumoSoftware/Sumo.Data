using System;

namespace Sumo.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityPrefixAttribute : Attribute
    {
        public EntityPrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; }
    }

    public class EntitySuffixAttribute : Attribute
    {
        public EntitySuffixAttribute(string suffix )
        {
            Suffix = suffix;
        }

        public string Suffix { get; }
    }
}
