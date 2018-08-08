using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    /// <summary>
    /// A schema is a namespace within a catalog. DBO is a a well known. It stands for database owner.
    /// In Oracle schemas are called databases. Oracle is weird. Don't use Oracle.
    /// </summary>
    [Serializable]
    public class Schema : Entity
    {
        public Schema() : base() { }
        public Schema(string name) : base(name) { }

        public Entity Owner { get; set; } = null;
        public List<Table> Tables { get; set; } = null;

        public Table AddTable(string name)
        {
            return AddTable(new Table(name));
        }

        public Table AddTable(Table table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (Tables == null) Tables = new List<Table>();

            Tables.Add(table);
            return table;
        }

        public override string ToString()
        {
            return $"{(Owner != null ? $"{Owner}." : string.Empty)}{base.ToString()}, Tables: {(Tables != null ? Tables.Count : 0)}";
        }
    }
}
