using System;
using System.Collections.Generic;
using System.Text;

namespace Sumo.Data
{
    public class ColumnName : ItemName, IColumnName, IItemName, IEquatable<ColumnName>
    {
        protected ColumnName() : base() { }

        public ColumnName(string name) : base(name) { }

        public ColumnName(string name, string alias) : base(name)
        {
            Alias = alias;
        }

        public string Alias { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ColumnName);
        }

        public bool Equals(ColumnName other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Alias == other.Alias;
        }

        public bool Equals(IColumnName other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Alias == other.Alias;
        }

        public override int GetHashCode()
        {
            var hashCode = 1209255309;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Alias);
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());
            if (!string.IsNullOrEmpty(Alias)) builder.Append($" as [{Alias}]");
            return builder.ToString();
        }

        public static bool operator ==(ColumnName name1, ColumnName name2)
        {
            return EqualityComparer<ColumnName>.Default.Equals(name1, name2);
        }

        public static bool operator !=(ColumnName name1, ColumnName name2)
        {
            return !(name1 == name2);
        }

        //todo: add null handling to all implicit operators. see EntityName for an example
        public static implicit operator string(ColumnName aliasedColumnName)
        {
            return aliasedColumnName.ToString();
        }
    }
}
