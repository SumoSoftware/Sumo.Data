using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    //https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite?view=msdata-sqlite-2.0.0
    public class Catalog : Entity
    {
        public Catalog() { }
        public Catalog(string name, string defaultOwner = null) : base(name)
        {
            DefaultOwner = defaultOwner;
        }

        public Entity DefaultOwner { get; set; } = null;

        public List<Schema> Schemas { get; set; } = null;

        public Schema AddSchema(string name, string owner = null)
        {
            return AddSchema(new Schema(name) { Owner = string.IsNullOrEmpty(owner) ? DefaultOwner : new Entity(owner) });
        }

        public Schema AddSchema(Schema schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));
            if (Schemas == null) Schemas = new List<Schema>();

            Schemas.Add(schema);
            return schema;
        }

        public override string ToString()
        {
            return $"{(DefaultOwner != null ? $"{DefaultOwner}." : string.Empty)}{base.ToString()}, Schemas: {(Schemas != null ? Schemas.Count : 0)}";
        }
    }
}
