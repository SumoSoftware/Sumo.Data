using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    //https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite?view=msdata-sqlite-2.0.0
    
    /// <summary>
    /// A catalog is a collection of schemas. It's the root entity of any database.
    /// </summary>
    [Serializable]
    public class CatalogDefinition : EntityDefinition
    {
        public CatalogDefinition() { }
        public CatalogDefinition(string name, string defaultOwner = null) : base(name)
        {
            DefaultOwner = defaultOwner;
        }

        public EntityDefinition DefaultOwner { get; set; } = null;

        public List<SchemaDefinition> Schemas { get; set; } = null;

        public SchemaDefinition AddSchema(string name, string owner = null)
        {
            return AddSchema(new SchemaDefinition(name) { Owner = string.IsNullOrEmpty(owner) ? DefaultOwner : new EntityDefinition(owner) });
        }

        public SchemaDefinition AddSchema(SchemaDefinition schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));
            if (Schemas == null) Schemas = new List<SchemaDefinition>();

            Schemas.Add(schema);
            return schema;
        }

        public override string ToString()
        {
            return $"{(DefaultOwner != null ? $"{DefaultOwner}." : string.Empty)}{base.ToString()}, Schemas: {(Schemas != null ? Schemas.Count : 0)}";
        }
    }
}
