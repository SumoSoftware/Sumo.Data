using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    /// <summary>
    /// A schema is a namespace within a catalog. DBO is a a well known. It stands for database owner.
    /// In Oracle schemas are called databases. Oracle is weird. Don't use Oracle.
    /// </summary>
    [Serializable]
    public class SchemaDefinition : EntityDefinition
    {
        public SchemaDefinition() : base() { }
        public SchemaDefinition(string name) : base(name) { }

        public EntityDefinition Owner { get; set; } = null;
        public List<TableDefinition> Tables { get; set; } = null;

        public TableDefinition AddTable(string name)
        {
            return AddTable(new TableDefinition(name));
        }

        public TableDefinition AddTable(TableDefinition table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (Tables == null) Tables = new List<TableDefinition>();

            Tables.Add(table);
            return table;
        }

        public override string ToString()
        {
            return $"{(Owner != null ? $"{Owner}." : string.Empty)}{base.ToString()}, Tables: {(Tables != null ? Tables.Count : 0)}";
        }
    }
}
