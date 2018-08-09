using System;
using System.Collections.Generic;
using System.Text;

namespace Sumo.Data
{
    public class EntityName : ItemName, IEntityName, IItemName, IEquatable<EntityName>
    {
        protected EntityName() : base() { }

        public EntityName(string name) : base(name)
        {
            Schema = String.Empty;
        }

        public EntityName(string schema, string name) : base(name)
        {
            if (String.IsNullOrEmpty(schema)) throw new ArgumentNullException(nameof(schema));

            Schema = schema;
        }

        public string Schema { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityName);
        }

        public bool Equals(EntityName other)
        {
            return other != null &&
                   Schema == other.Schema;
        }

        public bool Equals(IEntityName other)
        {
            return other != null &&
                   Schema == other.Schema;
        }

        public override int GetHashCode()
        {
            return 1951375558 + EqualityComparer<string>.Default.GetHashCode(Schema);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!String.IsNullOrEmpty(Schema)) builder.Append($"[{Schema}].");
            builder.Append($"[{Name}]");
            return builder.ToString();
        }

        public static bool operator ==(EntityName name1, EntityName name2)
        {
            return EqualityComparer<EntityName>.Default.Equals(name1, name2);
        }

        public static bool operator !=(EntityName name1, EntityName name2)
        {
            return !(name1 == name2);
        }

        public static implicit operator string(EntityName entityName)
        {
            return entityName != null ? entityName.ToString() : String.Empty ;
        }

        public static implicit operator EntityName(string entityName)
        {
            return String.IsNullOrEmpty(entityName) ? null : new EntityName(entityName);
        }
    }
}