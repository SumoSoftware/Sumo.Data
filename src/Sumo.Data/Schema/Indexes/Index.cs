using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class Index : Entity
    {
        public Index() : base() { }
        public Index(string name) : base(name) { }

        public bool IsUnique { get; set; } = false;
        public List<IndexedColumn> IndexedColumns { get; set; } = null;
        public List<Entity> CoveringColumns { get; set; } = null;
        public string FilterExpression { get; set; } = null;

        public IndexedColumn AddColumn(Column column, Directions direction = Directions.Ascending)
        {
            return AddColumn(new IndexedColumn(column, direction));
        }

        public IndexedColumn AddColumn(string name, Directions direction = Directions.Ascending)
        {
            return AddColumn(new IndexedColumn(name, direction));
        }

        public IndexedColumn AddColumn(IndexedColumn column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));
            if (IndexedColumns == null) IndexedColumns = new List<IndexedColumn>();

            column.OrdinalPosition = IndexedColumns.Count + 1;
            IndexedColumns.Add(column);
            return column;
        }

        public void AddCoveringColumn(string name)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (CoveringColumns == null) CoveringColumns = new List<Entity>();

            CoveringColumns.Add(new Entity(name));
        }

        public override string ToString()
        {
            
            return $"{base.ToString()}{(IsUnique ? " UNIQUE" : String.Empty)}{(IndexedColumns != null ? $" ON {string.Join(", ", IndexedColumns)}" : String.Empty)}";
        }
    }
}
