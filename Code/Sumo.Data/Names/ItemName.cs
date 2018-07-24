using System;
using System.Collections.Generic;

namespace Sumo.Data.Names
{
    public class ItemName : IItemName, IEquatable<ItemName>
    {
        protected ItemName() { }

        public ItemName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
        }
        public string Name { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ItemName);
        }

        public bool Equals(ItemName other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public bool Equals(IItemName other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public override string ToString()
        {
            return $"[{Name}]";
        }

        public static bool operator ==(ItemName name1, ItemName name2)
        {
            return EqualityComparer<ItemName>.Default.Equals(name1, name2);
        }

        public static bool operator !=(ItemName name1, ItemName name2)
        {
            return !(name1 == name2);
        }

        public static implicit operator string(ItemName itemName)
        {
            return itemName.ToString();
        }

        public static implicit operator ItemName(string name)
        {
            return new ItemName(name);
        }
    }
}
